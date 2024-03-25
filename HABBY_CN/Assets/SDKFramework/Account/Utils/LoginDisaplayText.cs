namespace SDKFramework.Account.Utils
{
    //只包含登录前的各种弹框
    public static class LoginDisaplayText
    {
        public static string Common_Header = "温馨提示";
        
        public static string Common_ErrorMsg = "ErrorCode:{0}, ErrMsg:{1}.";
        public static string Common_ErrorMsg_1 = "ErrMsg:{0}.";
        public static string Common_ErrorMsg_2 = "网络异常，请稍后重试。";

        public static string Error_NetworkFailure = "网络异常，请稍后重试。";
        public static string Error_IdentityFailure = "认证失败，请稍后再试。";
        public static string Error_InvalidAccountInfo = "用户名或密码错误，请重新登录。";
        public static string Error_InvalidIdentityInfo = "请输入正确的身份信息。";
        
        //第三方登录
        public static string Error_UserRejectAuthorization = "用户拒绝授权。";
        public static string Error_UserCancelAuthorization = "用户取消授权。";
        public static string Error_NotInstallApp = "未安装应用";

        public static string Common_App_Age_Tip_Header = "<xxxx>适龄提示";
        // 年龄显示
        public static string Common_App_Age_Tip_Content = "1、本游戏是一款弹射类即时策略手机游戏，适用于年满12周岁及以上的用户，建议未成年人在家长监护下使用游戏产品。\n" +
                                                          "2、本游戏以虚拟的遗迹大陆为背景，邪恶的方块军团占领了遗迹大陆，玩家为了打败方块军团，必须挺身而出对抗入侵者。玩家使用魔法弹珠，最终战胜了方块军团，让遗迹大陆恢复光明。游戏画面色彩鲜明，玩法简单，无社交功能。\n" +
                                                          "3、本游戏中有用户实名认证系统，认证为未成年人的用户将接受以下管理：\n" +
                                                          "游戏中部分道具需要付费。未满8周岁的用户不能付费；8周岁以上未满16周岁的未成年人用户，单次充值金额不得超过50元人民币，每月充值累计金额不得超过200元人民币；16周岁以上未满18岁的未成年人用户，单次充值金额不得超过100元人民币，每月充值累计金额不得超过400元人民币。\n" +
                                                          "未成年人用户仅可在周五、周六、周日和法定节假日每日20时至21时使用本游戏。" +
                                                          "4、游戏以弹球射击玩法为主，画面精美，能够带给玩家轻松愉悦的游戏氛围。游戏中玩家通过选择打击的角度，有策略地获得战斗的胜利，游戏的策略性可以充分发挥玩家的主动性，调动玩家的积极情绪。通过游戏，还能够锻炼玩家的思考力。";

    }
}