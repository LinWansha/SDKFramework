using System;

namespace Habby.CNUser
{
    public partial class AccountManager
    {

        //登录用户时候，发现用户不存在
        public static event Action OnUserNotExists;

        //创建用户失败
        public static event Action OnFailedToCreateUser;
        public static event Action OnLoginResponseSuccess;

        //没有得到用户信息，需要重新登录或注册
        public static event Action OnShowLoginScene;

        public static event Action<int> OnIdentityFailed; // 错误 
        public static event Action OnIdentitySuccess;

        /// <summary>
        ///  防沉迷登陆结果
        /// </summary>
        public static event Action<bool> OnAntiAddictionResultLogin;

        //======================================================

        public delegate void UserAction(UserAccount account);

        //登录用户服务器成功
        public static event UserAction OnUserLogin;

        //已阅读未成年协议（阅读确定后与才能进游戏）
        public static event UserAction OnReadedUnderAgeTip;

        //用户登出
        public static event Action OnUserLogout;

        //单次支付金额超过限制
        public static event UserAction OnExpenseOverRange;

        //单月支付金额超过限制
        public static event UserAction OnMonthlyExpenseOverRange;

        public static event UserAction OnLoginForbiddenTime;
        public static event UserAction OnTenMinutesLeft;
        public static event UserAction OnNoTimeLeft;

        public static event Action OnCloseUnderAgeNotice;
        public static event Action OnCloseTenMinutesLeftAndContinueGame;
        public static event Action OnCloseAndLogout;
        public static event Action OnCloseExpneseOverRange;
        public static event Action OnCloseNoGachaLeftForToday;
    }
}