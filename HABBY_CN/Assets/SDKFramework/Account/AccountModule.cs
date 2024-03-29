using SDKFramework.Account.AntiAddiction;
using SDKFramework.Account.DataSrc;
using static SDKFramework.Account.DataSrc.UserAccount;

namespace SDKFramework.Account
{
    public partial class AccountModule : BaseModule
    {
        internal string TAG = "[Account]";
        public bool IsLogin { get; private set; }

        private readonly AntiAddictionTimeChecker timeManager = new AntiAddictionTimeChecker();
        
        public bool HasAccount => CurrentAccount != null;
        
        public UserAccount CurrentAccount { get; private set; }
        
        private UserAccountHistory _AccountHistory;

        private UserAccountHistory AccountHistory
        {
            get
            {
                if (_AccountHistory != null) return _AccountHistory;
                if (!FileSaveLoad.HasAccountHistory)
                {
                    _AccountHistory = new UserAccountHistory();
                    _AccountHistory.Save();
                }
                else
                {
                    _AccountHistory = FileSaveLoad.LoadHistory();
                }

                return _AccountHistory;
            }
        }

        protected internal override void OnModuleInit()
        {
            base.OnModuleInit();
            Reload();
        }

        private void Reload()
        {
            CurrentAccount = FileSaveLoad.LoadAccount();
            HLogger.Log($"{TAG} Reload data UID={CurrentAccount?.UID} TotalIAP={CurrentAccount?.IAP.Total}, TodayOnline={CurrentAccount?.Online.Today}");
        }
        
        public void CheckUser()
        {
            if (HasAccount)
            {
                UserAccount account = CurrentAccount;
                HLogger.Log($"{TAG} checkUser token={account.AccessToken}");
                if (IsLogin) ShowLoginScene();
            }
            else
            {
                HLogger.Log($"{TAG} checkUser has no account info");
                ShowLoginScene();
            }
        }

        public void Save()
        {
            if (CurrentAccount == null) return;
            AccountHistory.SaveAccount(CurrentAccount);
            FileSaveLoad.SaveAccount(CurrentAccount);
#if USE_ANTIADDICTION
            timeManager.UploadData(CurrentAccount.Online);
#endif
        }

        public void ClearCurrent()
        {
            if (HasAccount)
            {
                FileSaveLoad.SaveAccount(null);
                CurrentAccount = null;
            }
        }
        
#region ANTIADDICTION
#if USE_ANTIADDICTION
        public bool CanPurchase(double amount) => PurchaseChecker.CanPurchase(CurrentAccount, amount);
        public void RefreshMonthlyExpense(double amount) => CurrentAccount?.RefreshMonthlyExpense(amount);

        public void Purchase(int amount)
        {
            HLogger.Log($"{TAG} --- Purchase add amount = {amount},nowMonth= {CurrentAccount.IAP.Monthly}" );
            CurrentAccount.AddIap(amount);
            HLogger.Log($"{TAG} --- Purchase add amount complete! nowMonth= {CurrentAccount.IAP.Monthly}");
        }

        public bool NoRightAge(UserAccount account)
        {
            return account.Age < (int)AppSource.Config.applicableRange;
        }
        public bool NoGameTime(UserAccount account)
        {
            HLogger.Log($"{TAG} ShouldForbidLogin age={account?.AgeRange}, time={(account?.Online != null ? account.Online.Today / 60 : 0)}");
            if (account == null) return true;
            return timeManager.IsBadTime(account);
        }

        public bool NoTimeLeft(UserAccount account)
        {
            HLogger.Log($"{TAG} HasTimeLeft age={account?.AgeRange} time = {(account?.Online != null ? account.Online.Today / 60 : 0)}");
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
        private int _update_interval = 100;

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
#endregion

#region Event

        public void FireCloseNoTime() => Logout();
        public void FireJuvenileEnterGame() => OnReadedAntiaddtionRules?.Invoke();
        public void FireExpenseOverRange() => OnSingleExpenseOverRange?.Invoke(LimitType.Single);
        public void FireMonthlyExpenseOverRange() => OnMonthlyExpenseOverRange?.Invoke(LimitType.Monthly);

 #endregion
    }
}