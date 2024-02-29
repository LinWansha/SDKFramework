namespace Habby.CNUser
{
    public class Response
    {
        public const int CODE_SUCCESS = 0;
        
        public const int CODE_PARAM_ERROR = 20001;//参数错误 通用
        public const int CODE_USER_NOT_FOUND = 20002;//未找到用户 登录
        public const int CODE_TOKEN_INVALID = 20003;//token不合法 通用
        public const int CODE_TOKEN_EXPIRE = 20004;//token失效 通用
        public const int CODE_LOGIN_ANOTHER_DEVICE = 20005;//登录新设备 ？
        public const int CODE_CONTENT_TYPE_INVALID = 20006;//    ？
        public const int CODE_PROTOCOL_INVALID = 20007;//解析错误 通用
        public const int CODE_BEING_ATTACKED = 20008;//进入战斗 ？
        public const int CODE_USER_FROZEN = 20009;//用户被冻结 通用
        // public const int CODE_ID_CARD_EXIST = 20010;//身份信息已认证 实名
        public const int CODE_NO_MATCHING_PLAYER = 20100;//没有匹配玩家 ？
        public const int CODE_WRONG_PASS = 20011;//密码错误
        public const int CODE_APP_TOKEN_EXPIRE = 20008;//token 过期 token过期，需要微信授权，获取新的code, 重新登录
        public const int CAPTCHA_INVALID = 20022;// 无效的验证码的
        public const int CAPTCHA_NO_FREE_TIME = 20021;// 没有免费发送短信次数
        //支付
        public const int CODE_IAP_NOT_FOUND = 20200;
        public const int CODE_RECEIPT_INVALID = 20201;
        public const int CODE_RECEIPT_REPEAT = 20202;

        //网络问题
        public const int CODE_SERVER_FATAL_ERROR = 30000;
        public const int CODE_SERVER_BUSY = 30001;
        public const int CODE_SERVER_NETWORK_ERROR = 30002;
        

        public int code;
        public string token;// 再次登陆使用
    }
    
    
}