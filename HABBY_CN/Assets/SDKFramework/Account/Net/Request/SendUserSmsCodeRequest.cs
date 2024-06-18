namespace SDKFramework.Account.Net
{
    public class SendUserSmsCodeRequest : Request
    {
        public string phone;
        public string accountType;
    }
}