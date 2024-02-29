namespace Habby.CNUser
{
    public class LoginRequest : Request
    {

        public const int TYPE_LOGIN_PASSWORD = 1;
        public const int TYPE_LOGIN_WECHAT = 2;
        public const int TYPE_LOGIN_GAMECENTER = 3;
        public const int TYPE_LOGIN_VISITOR = 4;
        
        public string socialId;
        public string deviceId;
        public string password;
        public string gcGameId;
        public string gcTeamId;
        public string token;
        public string accountType;
        public string thirdpartyCode;// 第三方 code
        // for qq
        public string openid;// 
        public string accessToken;// qq登陆用
        //for phone
        public string phone;// 
        public string captcha;// 
        //for appleid
        public string username;// 
        public string email;//
        public string appleUserId;  
        public string identityToken;
        public string mobileToken;// 手机快速登陆的token
        
    }
}