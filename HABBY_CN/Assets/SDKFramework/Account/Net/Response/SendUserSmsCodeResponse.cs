namespace SDKFramework.Account.Net
{
    public class SendUserSmsCodeResponse : Response
    {
        // public const int ERR_SMS_SERVICE = 50101;
        // public const int ERR_NUMBER_FORMAT = 50102;
        // public const int NUMBER_NOT_EXIST = 50103;
        
        // public const int TYPE_CHECK_EXISTS = 0;
        // public const int TYPE_REGISTER = 1;
        // public const int TYPE_LOGIN = 2;
        public const int CAPTCHA_EXCEEDED_TIMES = 20021; // 发送验证码的次数超限
        // public const int TYPE_RESET_PASSWORD = 3;
        public CodePair data;
    
    }
    
    public struct CodePair
    {
        // public string captcha;
        public int remaining; 
        public string uplinkSMS;
        public string countryPhone;
        public string captcha;
    }
}