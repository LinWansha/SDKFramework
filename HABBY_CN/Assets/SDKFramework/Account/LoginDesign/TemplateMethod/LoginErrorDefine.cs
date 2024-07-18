namespace SDKFramework.Account
{
    public static class LoginErrorConst
    {
        public const string USER_NOT_FOUND = "找不到此用户";
        public const string SMS_VERIFY_CODE_ERROR = "手机验证码错误";
        public const string OAUTH_EXPIRE = "{0} 授权过期,请重新授权";
        public const string LOGIN_FAILURE = "{0} 登录失败, 错误码: {1}";

        public const string IDENTITY_FAILURE = "实名认证失败!错误代码：{0}";
        public const string IDENTITY_WRONG_NAME = "您输入的姓名有误,请重新输入";
        public const string IDENTITY_WRONG_ID = "您输入的身份证号码有误,请重新输入";
        public const string IDENTITY_INPUT_ISNULL = "输入的姓名和身份证号不能为空";
        
        public const string PARAM_ERROR = "输入参数错误";
        public const string ID_CARD_EXIST = "此身份证已经绑定过其他账号";
        public const string SERVER_FATAL_ERROR = "GM 服务器故障";
        public const string SERVER_BUSY = "服务器繁忙";
        public const string IDENTITY = "认证失败";
        public const string ID_CARD_OVER_COUNT = "认证次数超限";
        public const string ID_CARD_CHECK_PENDING = "认证中！稍后再试";
        public const string GAME_SERVER_ERROR = "游戏服务器故障";
        public const string UN_KNOW = "未知错误 错误码";
    }
}