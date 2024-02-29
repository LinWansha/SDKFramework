namespace Habby.CNUser
{
    public class IdentityResponse : Response
    {
        public const int ERROR = 1;//未知错误
        public const int PARAM_ERROR = 20001; // 参数错误
        public const int USER_NOT_FOUND = 20004;// 找不到此用户
        public const int ID_CARD_EXIST = 20006; // 身份证号已存在
        public const int TOKEN_EXPIRE = 20008;// token 过期

        public const int ID_CARD_CHECK_PENDING = 20009; // 实名认证， 认证中
        public const int ID_CARD_CHECK_FAILED = 20010; //  实名认证， 认证失败
        public const int ID_CARD_OVER_COUNT = 20011; //  实名认证次数超限
        public const int SERVER_FATAL_ERROR = 30000; // GM 服务器故障
        public const int SERVER_BUSY = 30001; // 服务器繁忙
        public const int GAME_SERVER_ERROR = 40000; // 游戏服务器故障
        
        
        public int code;
        public bool validated;

        public IdentityData data;
    }
    
    public class IdentityData
    {
        public int age;
        public int addictLevel;
    }
}