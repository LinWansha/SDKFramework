using System;

namespace Habby.CNUser
{
    public partial class AccountManager
    {
        /*===================Login Or Register====================*/
        public static event Action OnUserLogin;                     //登录用户服务器成功
        
        public static event Action OnUserLogout;                    //用户登出
        
        public static event Action OnFailedToCreateUser;            //创建用户失败
        
        public static event Action OnUserNotExists;                 //登录用户时候，发现用户不存在

        public static event Action OnLoginResponseSuccess;          //用户存在
        
        public static event Action OnShowLoginScene;                //没有得到用户信息，需要重新登录或注册
        
        /*=====================Anti-addiction======================*/

        public static event Action<int> OnIdentityFailed;           //实名认证出错

        public static event Action OnIdentitySuccess;               //实名成功

        public static event Action<bool> OnAntiAddictionResultLogin;//  防沉迷登陆结果

        /*======================================================*/
        
        public delegate void UserAction(UserAccount account);
        

        public static event Action OnReadedUnderAgeTip;         //已阅读未成年协议（阅读确定后与才能进游戏）

        public static event Action<LimitType> OnSingleExpenseOverRange;          //单次支付金额超过限制

        public static event Action<LimitType>  OnMonthlyExpenseOverRange;   //单月支付金额超过限制

        public static event Action OnNoTimeLeft;                //当日游戏时间用尽
    }
}