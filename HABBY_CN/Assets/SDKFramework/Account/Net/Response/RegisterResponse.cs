namespace SDKFramework.Account.Net
{
    public class RegisterResponse : Response
    {
        public RegisterPair data;
    }

    public struct RegisterPair
    {
        public string socialId;
        public string password;
    }
}