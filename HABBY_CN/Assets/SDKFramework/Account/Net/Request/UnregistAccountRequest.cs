namespace SDKFramework.Account.Net
{
    public class UnregistAccountRequest: Request
    {
        public string accountType;
        public string token;
        public string code;
    }
}