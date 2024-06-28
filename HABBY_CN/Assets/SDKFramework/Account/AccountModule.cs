using System;
using System.Collections.Generic;
using SDKFramework.Account.AntiAddiction;
using SDKFramework.Account.DataSrc;
using SDKFramework.Message;


namespace SDKFramework.Account
{
    public partial class AccountModule
    {
        public Dictionary<string, LoginChannel> LoginMethodMap = new Dictionary<string, LoginChannel>()
        {
            {UserAccount.ChannelQQ,LoginChannel.QQ},
            {UserAccount.ChannelPhone,LoginChannel.Phone},
            {UserAccount.ChannelWeiXin,LoginChannel.WX},
            {UserAccount.ChannelAppleId,LoginChannel.Apple},
            {UserAccount.ChannelEditor,LoginChannel.Editor},
        };
        public bool IsLogin { get; private set; }

        public string LoginSessionId { get; private set; }

        private void RefreshLoginSessionId() => Guid.NewGuid().ToString();
        
        public bool IsLoginStateDirty { get; private set; }
        
        internal DecisionMaker loginRunner = new DecisionMaker();

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
            Save();
            AccountLog.Info($"--- onUserLogout try crash!");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }

        private void onUserLogin()
        {
            AccountLog.Info($"onUserLogin登录成功");
            HabbyFramework.UI.CloseUI(UIViewID.EntryUI);
            HabbyFramework.UI.OpenUI(UIViewID.LoginSuccessUI);
            HabbyFramework.Message.Post(new SDKEvent.SDKLoginFinish() { code = 0,msg = "success",uid = CurrentAccount.UID,isNew = CurrentAccount.IsNewUser});
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

        public void Save()
        {

            AccountHistory.SaveAccount(CurrentAccount);
            FileSaveLoad.SaveAccount(CurrentAccount);

#if USE_ANTIADDICTION
            timeManager.UploadData(CurrentAccount.Online);
#endif
        }
        
        public void SetCurrentAccount(UserAccount account)
        {
            if (account != null)
                CurrentAccount = account;
        }
        
        public void ClearCurrent()
        {
            if (!HasAccount) return;
            FileSaveLoad.SaveAccount(null);
            AccountHistory.Delete(CurrentAccount);
            CurrentAccount = new UserAccount();
        }
    }
}