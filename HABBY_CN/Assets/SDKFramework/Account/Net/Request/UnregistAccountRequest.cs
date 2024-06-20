namespace SDKFramework.Account.Net
{
    public class UnregistAccountRequest: Request
    {
        public string accountType;
        public new string token;
        public string code;
    }
}