namespace SDKFramework.Account.Net
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