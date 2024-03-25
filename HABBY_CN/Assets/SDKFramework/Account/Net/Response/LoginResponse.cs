namespace SDKFramework.Account.Net
{
    public class LoginResponse : Response
    {
        public LoginRespData data;
    }
    
    public struct LoginRespData
    {

        public int validateIdentity;

        public int totalOnlineTime;
        public int todayOnlineTime;

        public int totalPaymentAmount;
        public int monthlyPaymentAmount;
        public int todayPaymentAmount;

        public int age;

        public string nickname;
        
        // public string unionId;
        public string userId;
        public string token;
        public int addictLevel;
        
        //新增字段
        public string lastHeartBeatAt;
        public string serverTime;
        public string openId; //微信平台用户id
        public string unionId; //微信平台用户Unionid
        public bool isNewUser;//是否是新注册用户
        public string phone;// 手机号
    }
}