namespace Habby.CNUser
{
    public class RegisterRequest : Request
    {
        public string customSocialId;
        public string customPassword;
        public string deviceId;
        public RegisterRequest()
        {
        }
    }
}