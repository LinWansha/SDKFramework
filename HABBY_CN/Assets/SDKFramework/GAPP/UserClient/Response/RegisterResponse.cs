namespace Habby.CNUser
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