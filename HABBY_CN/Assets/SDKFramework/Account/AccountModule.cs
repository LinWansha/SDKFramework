using System;
using SDKFramework.Account.AntiAddiction;
using SDKFramework.Account.DataSrc;


namespace SDKFramework.Account
{
    public partial class AccountModule
    {
        public bool IsLogin { get; private set; }

        private readonly AntiAddictionTimeChecker timeManager = new AntiAddictionTimeChecker();
        
        public bool HasAccount => CurrentAccount != null && CurrentAccount.LoginChannel!=null;
        
        public UserAccount CurrentAccount { get; private set; }
        
        private UserAccountHistory _AccountHistory;

        public UserAccountHistory AccountHistory
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
        
        /*=================== Login Or Register ====================*/
        internal static event Action OnUserLogin; //登录用户服务器成功

        internal static event Action OnUserLogout; //用户登出

        internal static event Action OnShowLoginScene; //没有得到用户信息，需要重新登录或注册

        /*===================== Validate Identity ====================*/
        internal static event Action<bool,int> OnValidateIdentityResult; //  防沉迷登陆结果

        /*===================== Anti-addiction ======================*/

        internal delegate void UserAction(UserAccount account);


        internal static event Action OnReadedAntiaddtionRules; //已阅读未成年协议（阅读确定后与才能进游戏）

        internal static event Action<LimitType> OnSingleExpenseOverRange; //单次支付金额超过限制

        internal static event Action<LimitType> OnMonthlyExpenseOverRange; //单月支付金额超过限制

        internal static event Action OnNoTimeLeft; //当日游戏时间用尽
        /*======================================================*/

        protected internal override void OnModuleStart()
        {
            base.OnModuleStart();
            AccountModule.OnUserLogin += onUserLogin;
            AccountModule.OnUserLogout += onUserLogout;
            AccountModule.OnShowLoginScene += ShowLoginScene;
        }

        protected internal override void OnModuleStop()
        {
            base.OnModuleStop();
            AccountModule.OnUserLogin -= onUserLogin;
            AccountModule.OnUserLogout -= onUserLogout;
            AccountModule.OnShowLoginScene -= ShowLoginScene;
        }

        private void onUserLogout()
        {
            AccountLog.Info($"--- onUserLogout try crash!");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }

        private void onUserLogin()
        {
            HabbyFramework.UI.CloseUI(UIViewID.EntryUI);
            HabbyFramework.UI.OpenUI(UIViewID.LoginSuccessUI);
            
            if (CurrentAccount?.AgeRange == UserAccount.AgeLevel.Adult)
            {
                // SDK.Procedure?.EnterGame();
            }

            AccountLog.Info($"onUserLogin登录成功");
        }

        private void ShowLoginScene()
        {
            HabbyFramework.UI.OpenUISingle(UIViewID.EntryUI);
            AccountLog.Info($"ShowLoginScene");
        }

        private void onClearUserCache()
        {
            AccountLog.Info($"--- onClearUserCache");
            AccountHistory.DeleteHistory();
            ClearCurrent();
        }
        
        private void Save()
        {
            if (CurrentAccount == null) return;
            AccountHistory.SaveAccount(CurrentAccount);
            FileSaveLoad.SaveAccount(CurrentAccount);
#if USE_ANTIADDICTION
            timeManager.UploadData(CurrentAccount.Online);
#endif
        }

        private void ClearCurrent()
        {
            if (HasAccount)
            {
                FileSaveLoad.SaveAccount(null);
                CurrentAccount = null;
            }
        }
    }
}