using System;
using static Habby.CNUser.UserAccount;
using Debug = UnityEngine.Debug;

namespace Habby.CNUser
{
    public partial class AccountManager : MonoSingleton<AccountManager>
    {
        //刷新间隔（帧为单位）
        private int _update_interval = 100;

        private int _update_count = 0;

        private bool isLogin = false;
        public bool IsLogin => isLogin;

        private uint _reloginFailCount;
        public void UpCountReloginFail() => _reloginFailCount++;
        public uint ReloginFailCount => _reloginFailCount;


        public readonly AntiAddictionTimeChecker timeManager = new AntiAddictionTimeChecker();

        public void Awake()
        {
            _reloginFailCount = 0;
            DontDestroyOnLoad(gameObject);
            Debug.unityLogger.logEnabled = true;
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
            switch (regulation)
            {
                case AntiAddictionTimeChecker.TimeRegulation.Exit:
                    // timeManager.UploadData(account.Online);
                    OnNoTimeLeft?.Invoke();
                    break;
            }
            
        }
#endif
        public void CheckAndClean()
        {
            // 非login状态才触发
            if (!isLogin)
            {
                if (null == CurrentAccount)
                {
                    Reload();
                }

                // 非空才检查，为了兼容旧版本没有数据的情况
                if (!string.IsNullOrEmpty(CurrentAccount.LoginServerName) &&
                    CurrentAccount.LoginServerName != "Http.URL")
                {
                    ClearCurrent();
                }

                if (AccountHistory.HasAccountHistory)
                {
                    AccountHistory.CheckAndClean();
                }
            }
        }

        public void Reload()
        {
            CurrentAccount = FileSaveLoad.LoadAccount();
            HLogger.LogFormat("UserAccoutManager Reload data UID={0} IAP={1}, IAP={2}", CurrentAccount?.UID,
                CurrentAccount?.IAP, CurrentAccount?.Online);
            _update_count = 0;
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

        public void SaveAccountInfo(UserAccount account)
        {
            HLogger.LogFormat("UserAccoutManager SaveAccountInfo account={0}, age={1}", account?.AccessToken,
                account?.AgeRange);
            account.LoginState = UserAccount.UserLoginState.Logout;
            CurrentAccount = account;
            FileSaveLoad.SaveAccount(account);
        }

        public void ClearAccountInfo()
        {
            HLogger.LogFormat("UserAccoutManager ClearAccountInfo account={0}, age={1}", CurrentAccount?.AccessToken,
                CurrentAccount?.AgeRange);
            if (CurrentAccount != null) timeManager.StopTimeCounter(CurrentAccount);
            CurrentAccount = null;
        }

        public void Login(UserAccount account)
        {
            HLogger.Log(string.Format("--- UserAccoutManager Login account={0}, age={1}", account?.AccessToken,
                account?.AgeRange));
            account.LoginState = UserAccount.UserLoginState.Logedin;
            account.SaveLoginTime();
            CurrentAccount = account;
            FileSaveLoad.SaveAccount(account);
            AccountHistory.SaveAccount(account);
#if USE_ANTIADDICTION
            account.Online?.Refresh();
            timeManager.StartTimeCounter();

            account.Gacha?.Refresh();
            account.IAP?.Refresh();
#endif
            isLogin = true;
            OnUserLogin?.Invoke();
            _reloginFailCount = 0; // reset fail count when login success
        }

        public void Logout(bool dispatch = true, bool removeUserAccount = false)
        {
            HLogger.LogFormat("UserAccountManager Logout, account={0}", CurrentAccount);
            if (CurrentAccount != null)
            {
                CurrentAccount.LoginState = UserAccount.UserLoginState.Logout;
#if USE_ANTIADDICTION
                timeManager.StopTimeCounter(CurrentAccount);
#endif
                (removeUserAccount ? (Action)ClearCurrent : Save)();
            }

            isLogin = false;

            HLogger.LogWarnFormat("--- Logout dispatch=" + dispatch);
            (dispatch ? OnUserLogout : OnShowLoginScene)?.Invoke();
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

        public void ReloadHistory()
        {
            _AccountHistory = FileSaveLoad.LoadHistory();
            if (_AccountHistory == null)
            {
                _AccountHistory = new UserAccountHistory();
            }
        }
#if USE_ANTIADDICTION
        public bool CanGacha(string gacha) => PurchaseChecker.CanGacha(CurrentAccount, gacha);
        public void Gacha(string gacha, int amount) => CurrentAccount.AddGacha(gacha, amount);
        public bool CanPurchase(double amount) => PurchaseChecker.CanPurchase(CurrentAccount, amount);
        public void Purchase(int amount)
        {
            HLogger.LogWarnFormat("--- Purchase add amount=" + amount + ",nowMonth=" + CurrentAccount.IAP.Monthly);
            CurrentAccount.AddIap(amount);
            HLogger.LogWarnFormat("--- Purchase add amount complete! nowMonth=" + CurrentAccount.IAP.Monthly);
        }

        public bool IsRestrictedTime(UserAccount account)
        {
            HLogger.LogFormat("ShouldForbidLogin age={0}, time={1}", account?.AgeRange,
                account.Online != null ? account.Online.Today / 60 : 0);
            if (account == null) return true;
            return timeManager.IsBadTime(account);
        }

        public bool HasTimeLeft(UserAccount account)
        {
            HLogger.LogFormat("HasTimeLeft age={0}, time={1}", account?.AgeRange,
                account.Online != null ? account.Online.Today / 60 : 0);
            if (account == null) return true;
            return timeManager.HasTimeLeft(account);
        }
#endif


        #region Event

        public void FireCloseNoTime() => Logout();
        public void FireCloseUnderAgePipPop() => OnReadedUnderAgeTip?.Invoke();
        public void FireExpenseOverRange() => OnSingleExpenseOverRange?.Invoke(LimitType.Single);
        public void FireMonthlyExpenseOverRange() => OnMonthlyExpenseOverRange?.Invoke(LimitType.Monthly);

        #endregion
    }
}