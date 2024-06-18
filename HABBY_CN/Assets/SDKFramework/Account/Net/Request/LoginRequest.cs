namespace SDKFramework.Account.Net
{
    [System.Serializable]
    public struct AppleIdUserInfo
    {
        public string email;
        public string namePrefix;
        public string givenName;
        public string middleName;
        public string familyName;
        public string nameSuffix;
        public string nickname;
    }
    
    public class LoginRequest : Request
    {
        public string socialId;
        public string deviceId;
        public string password;
        public string gcGameId;
        public string gcTeamId;
        public new string token;
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
        public AppleIdUserInfo user;
    }
}