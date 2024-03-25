namespace SDKFramework.Account.Utils
{
    public static class AntiAddictionDisaplayText
    {
        public static string Common_Header = "温馨提示";
        
        public static string Common_ErrorMsg = "ErrorCode:{0}, ErrMsg:{1}.";
        public static string Common_ErrorMsg_1 = "ErrMsg:{0}.";
        public static string Common_ErrorMsg_2 = "网络异常，请稍后重试。";
        
        public static string NoTimeLeftPopup_Vistor = "<color=#FFFFFF00>缩进</color>系统识别到您使用游客身份的时间超过限制，\n请登陆之后再继续游戏。";

        public static string NoTimeLeft = "您好：检测到您目前为未成年人账号，已被纳入防沉迷\n系统，根据国家新闻出版署《关于防止未成年人沉迷网\n络游戏的通知》《关于进一步严格管理切实防止未成年\n人沉迷网络游戏的通知》要求，本游戏仅在周五、周六、\n周日和法定节假日每日20时至21时向未成年人提供1小\n时服务，其他时间未成年人均不能登录本游戏。";

        public static string NoGameTime = "您目前为未成年人账号，已被纳入防沉迷系统，根据\n国家新闻出版署《关于防止未成年人沉迷网络游戏的\n通知》《关于进一步严格管理切实防止未成年人沉迷\n网络游戏的通知》要求，本游戏仅在周五、周六、周\n日和法定节假日每日20时至21时向未成年人提供1小\n时服务，其他时间未成年人均不能登录本游戏。";
        
        public static string NoRightAge = "您属于未满{0}周岁的未成年人，已被纳入防沉迷系统。\n为严格防止未成年人沉迷网络游戏，遵照本游戏的分级\n划分，本游戏将无法为您提供游戏服务。";

        public static string MonthRemain = "<color=#fd4444>本月剩余充值金额：{0}</color>"; 
        
        public static string PurchaseNotice_NotLogin = "您未登录，无法进行游戏充值。"; 
        public static string PurchaseNotice_SpendUnder8 = "您未满8周岁，无法进行游戏充值。";
        public static string PurchaseNotice_SpendUnder16 = "您未满16周岁，单次充值额度不得超过50元。";
        public static string PurchaseNotice_SpendUnder18 = "您未满18周岁，单次充值额度不得超过100元。";
        public static string PurchaseNotice_MonthlySpendUnder16 = "您未满16周岁，每月累计充值额度不得超过200元，未\n成年账号已达到充值额度，暂无法充值。";
        public static string PurchaseNotice_MonthlySpendUnder18 = "您未满18周岁，每月累计充值额度不得超过400元，未\n成年账号已达到充值额度，暂无法充值。";
        
        public static string TeenagerNotice = "系统识别到您未满18岁，为未成年人。\n" +
                                            "为了保护未成年人的身心健康，国家新闻出版署发布了\n"+
                                            "《关于防止未成年人沉迷网络游戏的通知》，部分规则\n"+
                                            "如下：\n"+
                                            "1.所有玩家账号均须使用实名注册，否则将无法享受游\n"+
                                            "戏服务。\n"+
                                            "2.防沉迷设置。\n"+
                                            "严格控制未成年人使用网络游戏时段、时长。\n"+
                                            "本游戏仅在周五、周六、周日和法定节假日每日20时至\n"+
                                            "21时向未成年人提供1小时服务，其他时间未成年人均\n"+
                                            "不能登录本游戏。  \n"+
                                            "3.规范向未成年人提供付费服务。\n"+
                                            "游戏不为未满8周岁的用户提供游戏付费服务。\n"+
                                            "8周岁以上未满16周岁的用户，在游戏中单次充值金额\n"+
                                            "不得超过50元，每月充值累计金额不得超过200元。\n"+
                                            "16周岁以上未满18岁的用户，在游戏中单次充值金额不\n"+
                                            "得超过100元，每月充值累计金额不得超过400元。\n";

        public static string Purchase_TeenagerNotice = "<color=#FFFFFF00>缩进</color>根据国家新闻出版署发布的《关于防止未成年人沉迷网络游戏的通知》，为了规范向未成年人提供付费服务，特规定游戏服务规则如下：\n" +
                                                       "<color=#FFFFFF00>缩进</color>游戏不为未满8周岁的用户提供游戏付费服务。\n" +
                                                       "<color=#FFFFFF00>缩进</color>8周岁以上未满16周岁的用户，在游戏中单次充值金额不得超过50元，每月充值累计金额不得超过200元。\n" +
                                                       "<color=#FFFFFF00>缩进</color>16周岁以上未满18岁的用户，在游戏中单次充值金额不得超过100元，每月充值累计金额不得超过400元。";
        
        public static string PurchaseNotice = "<color=#FFFFFF00>缩进</color>系统识别到您未满18岁，为未成年人，为了保护未成年人身心健康，国家新闻出版署发布了《关于防止未成年人沉迷网络游戏的通知》，部分规则如下：\n" +
                                              "<color=#FFFFFF00>缩进</color>游戏不为未满8周岁的用户提供游戏付费服务。\n" +
                                              "<color=#FFFFFF00>缩进</color>8周岁以上未满16周岁的用户，在游戏中单次充值金额不得超过50元，每月充值累计金额不得超过200元。\n" +
                                              "<color=#FFFFFF00>缩进</color>16周岁以上未满18岁的用户，在游戏中单次充值金额不得超过100元，每月充值累计金额不得超过400元。";
        
        public static string Error_NetworkFailure = "网络异常，请稍后重试。";
        public static string Error_LoginStateFailure = "登录失效，请重新登录。";
        public static string Error_ForbidLogin = "您目前为未成年人账号，已被纳入防沉迷系统，根据国\n"+
                                                "家新闻出版署《关于防止未成年人沉迷网络游戏的通\n"+
                                                "知》，本游戏仅在周五、周六、周日和法定节假日每日\n"+
                                                "20时至21时向未成年人提供1小时服务，其他时间未成\n"+
                                                "年人均不能登录本游戏。";
        public static string Normal_RemainTimeNotice = "<color=#FFFFFF00>缩进</color>尊敬的玩家您好，根据国家规定，未成年玩家非节假日时间每日游戏时长不得超过1.5小时。";
        public static string Holiday_RemainTimeNotice = "<color=#FFFFFF00>缩进</color>尊敬的玩家您好，根据国家规定，未成年玩家节假日时间每日游戏时长不得超过3.0小时。";
        public static string TimeRemain = "<color=#fd4444>当前剩余在线时间：{0}</color>";

    }
}

// {EPopupType.ThirdPartyFailure, "第三方登录失败。"},
// {EPopupType.CannotCreateUser, "您登录的账号不存在，请重新登录"},
// {EPopupType.UserNotExists, "用户不存在，请重新登录"},
// {EPopupType.FailedToRegister, "用户注册失败，请重试"},
// {EPopupType.InvalidAccountInfo, "用户名或密码错误，请重新登录"},