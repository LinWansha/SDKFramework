using System;
using SDKFramework.Account.DataSrc;
using UnityEngine;


namespace SDKFramework.Account
{
    public partial class AccountModule
    {
        /*===================Login Or Register====================*/
        internal static event Action OnUserLogin; //登录用户服务器成功

        internal static event Action OnUserLogout; //用户登出

        internal static event Action OnShowLoginScene; //没有得到用户信息，需要重新登录或注册

        /*=====================Anti-addiction======================*/

        internal static event Action<int> OnIdentityFailed; //实名认证出错

        internal static event Action OnIdentitySuccess; //实名成功

        internal static event Action<bool> OnAntiAddictionResultLogin; //  防沉迷登陆结果

        /*======================================================*/

        internal delegate void UserAction(UserAccount account);


        internal static event Action OnReadedAntiaddtionRules; //已阅读未成年协议（阅读确定后与才能进游戏）

        internal static event Action<LimitType> OnSingleExpenseOverRange; //单次支付金额超过限制

        internal static event Action<LimitType> OnMonthlyExpenseOverRange; //单月支付金额超过限制

        internal static event Action OnNoTimeLeft; //当日游戏时间用尽

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
#if MRQ
            HabbyFramework.UI.OpenUI(UIViewID.LoginSuccessUI);
#endif
            if (CurrentAccount?.AgeRange == UserAccount.AgeLevel.Adult)
            {
                SDK.Procedure?.EnterGame();
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
    }
}