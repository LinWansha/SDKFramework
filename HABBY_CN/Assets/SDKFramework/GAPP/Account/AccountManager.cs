using System;
using SDKFramework;
using SDKFramework.Utils;
using static Habby.CNUser.UserAccount;
using Debug = UnityEngine.Debug;

namespace Habby.CNUser
{
    public partial class AccountManager : MonoSingleton<AccountManager>
    {
        private bool isLogin = false;
        public bool IsLogin => isLogin;
        
        public readonly AntiAddictionTimeChecker timeManager = new AntiAddictionTimeChecker();

        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Reload();
        }
#if USE_ANTIADDICTION
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
        void Update()
        {
            //成年玩家及未登录状态不作处理
            if (CurrentAccount == null || !isLogin) return;
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

        public void Reload()
        {
            CurrentAccount = FileSaveLoad.LoadAccount();
            HLogger.LogFormat("UserAccoutManager Reload data UID={0} TotalIAP={1}, TodayOnline={2}", CurrentAccount?.UID,
                CurrentAccount?.IAP.Total, CurrentAccount?.Online.Today);
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
        
        public void Login(UserAccount account)
        {
            HLogger.Log(string.Format("--- UserAccoutManager Login account={0}, age={1}", account?.AccessToken,
                account?.AgeRange));
            if (account == null) return;
            account.SaveLoginTime();
            CurrentAccount = account;
            FileSaveLoad.SaveAccount(account);
            AccountHistory.SaveAccount(account);
#if USE_ANTIADDICTION
            account.IAP?.Refresh();
            account.Online?.Refresh();
            timeManager.StartTimeCounter();
            _ = (CurrentAccount.AgeRange != AgeLevel.Adult) ? gameObject.AddComponent<GAPPListener>() : null;
#endif
            isLogin = true;
            OnUserLogin?.Invoke();
        }

        public void Logout(int actionCode = 0)
        {
            HLogger.LogFormat("UserAccountManager Logout, account={0}", CurrentAccount);
            if (CurrentAccount != null)
            {
#if USE_ANTIADDICTION
                timeManager.StopTimeCounter(CurrentAccount);
#endif
                //TODO：这部分逻辑应该重新整理，版署可以这样做，线上切换账号时登出，不会在本地清掉当前账号数据存档
                (actionCode == 0 ? (Action)Save : ClearCurrent)();
            }

            isLogin = false;

            (actionCode == 0 ? OnUserLogout : OnShowLoginScene)?.Invoke();
            HLogger.LogWarnFormat("--- Logout ActionCode=" + actionCode);
        }

        public void ClearCurrent()
        {
            if (HasAccount)
            {
                FileSaveLoad.SaveAccount(null);
                CurrentAccount = null;
            }
        }

        public bool HasAccount => CurrentAccount != null;
        public UserAccount CurrentAccount { get; private set; }

        private UserAccountHistory _AccountHistory;

        public UserAccountHistory AccountHistory
        {
            get
            {
                if (_AccountHistory == null)
                {
                    if (!FileSaveLoad.HasAccountHistory)
                    {
                        _AccountHistory = new UserAccountHistory();
                        _AccountHistory.Save();
                    }
                    else
                    {
                        _AccountHistory = FileSaveLoad.LoadHistory();
                    }
                }

                return _AccountHistory;
            }
        }

#if USE_ANTIADDICTION
        public bool CanPurchase(double amount) => PurchaseChecker.CanPurchase(CurrentAccount, amount);

        public void Purchase(int amount)
        {
            HLogger.LogWarnFormat("--- Purchase add amount=" + amount + ",nowMonth=" + CurrentAccount.IAP.Monthly);
            CurrentAccount.AddIap(amount);
            HLogger.LogWarnFormat("--- Purchase add amount complete! nowMonth=" + CurrentAccount.IAP.Monthly);
        }

        public bool NoRightAge(UserAccount account)
        {
            return account.Age < (int)AppSource.Data.applicableRange;
        }
        public bool NoGameTime(UserAccount account)
        {
            HLogger.LogFormat("ShouldForbidLogin age={0}, time={1}", account?.AgeRange,
                account?.Online != null ? account.Online.Today / 60 : 0);
            if (account == null) return true;
            return timeManager.IsBadTime(account);
        }

        public bool NoTimeLeft(UserAccount account)
        {
            HLogger.LogFormat("HasTimeLeft age={0}, time={1}", account?.AgeRange,
                account?.Online != null ? account.Online.Today / 60 : 0);
            if (account == null) return false;
            return !timeManager.HasTimeLeft(account);
        }
#endif


        #region Event

        public void FireCloseNoTime() => Logout();
        public void FireJuvenileEnterGame() => OnReadedAntiaddtionRules?.Invoke();
        public void FireExpenseOverRange() => OnSingleExpenseOverRange?.Invoke(LimitType.Single);
        public void FireMonthlyExpenseOverRange() => OnMonthlyExpenseOverRange?.Invoke(LimitType.Monthly);

        #endregion
    }
}