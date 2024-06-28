using SDKFramework.Account.AntiAddiction;
using SDKFramework.Account.DataSrc;
using static SDKFramework.Account.DataSrc.UserAccount;

namespace SDKFramework.Account
{
    public partial class AccountModule : BaseModule
    {
        private bool CanLogin(UserAccount account)
        {
#if USE_ANTIADDICTION
            ExitReason? reason =
                NoRightAge(account) ? ExitReason.NoRightAge :
                NoGameTime(account) ? ExitReason.NoGameTime :
                NoTimeLeft(account) ? ExitReason.NoTimeLeft :
                (ExitReason?)null;

            if (reason != null)
            {
                HabbyFramework.UI.OpenUI(UIViewID.CrashUI, reason.Value);
            }

            return reason == null;
#else
            return true;
#endif
        }
        
#if USE_ANTIADDICTION
        
        public bool CanPurchase(double amount) => PurchaseChecker.CanPurchase(CurrentAccount, amount);

        public void RefreshIAP(double total,double monthly)
        {
            CurrentAccount?.RefreshTotalExpense(total);
            CurrentAccount?.RefreshMonthlyExpense(monthly);
            HabbyFramework.Analytics.RefreshCommonProperties();
        }

        public void Purchase(int amount)
        {
            AccountLog.Info($"--- Purchase add amount = {amount},nowMonth= {CurrentAccount.IAP.Monthly}" );
            CurrentAccount.AddIap(amount);
            AccountLog.Info($"--- Purchase add amount complete! nowMonth= {CurrentAccount.IAP.Monthly}");
        }

        private bool NoRightAge(UserAccount account)
        {
            return account.Age < (int)Global.App.applicableRange;
        }

        private bool NoGameTime(UserAccount account)
        {
            AccountLog.Info($"ShouldForbidLogin age={account?.AgeRange}, time={(account?.Online != null ? account.Online.Today / 60 : 0)}");
            if (account == null) return true;
            return timeManager.IsBadTime(account);
        }

        private bool NoTimeLeft(UserAccount account)
        {
            AccountLog.Info($"HasTimeLeft age={account?.AgeRange} time = {(account?.Online != null ? account.Online.Today / 60 : 0)}");
            if (account == null) return false;
            return !timeManager.HasTimeLeft(account);
        }
        
        public void OnApplicationPause(bool pause)
        {
            if (CurrentAccount == null) return;

            if (pause)
            {
                timeManager.StopTimeCounter(CurrentAccount);
            }
            else
            {
                timeManager.StartTimeCounter();
                CurrentAccount.Online?.Refresh();
            }
        }
        //刷新间隔（帧为单位）
        private readonly byte _update_interval = 100;

        private int _update_count = 0;
        protected internal override void OnModuleUpdate(float deltaTime)
        {
            base.OnModuleUpdate(deltaTime);
            //成年玩家及未登录状态不作处理
            if (CurrentAccount == null || !IsLogin) return;
            if (CurrentAccount.AgeRange == AgeLevel.Adult)
            {
                // 刷新下在线时间(发心跳包)
                timeManager.CheckOnlineTime(CurrentAccount);
                return;
            }

            if (++_update_count < _update_interval) return;
            _update_count = 0;

            CheckRegulation(CurrentAccount);
        }

        public void CheckRegulation(UserAccount account)
        {
            AntiAddictionTimeChecker.TimeRegulation regulation = timeManager.CheckOnlineTime(account);
            if (regulation==AntiAddictionTimeChecker.TimeRegulation.Exit)
            {
                timeManager.UploadData(account.Online);
                OnNoTimeLeft?.Invoke();
            }
        }
#endif

        public void FireCloseNoTime() => Logout();
        public void FireJuvenileEnterGame() => OnReadedAntiaddtionRules?.Invoke();
        public void FireExpenseOverRange() => OnSingleExpenseOverRange?.Invoke(LimitType.Single);
        public void FireMonthlyExpenseOverRange() => OnMonthlyExpenseOverRange?.Invoke(LimitType.Monthly);

    }
}