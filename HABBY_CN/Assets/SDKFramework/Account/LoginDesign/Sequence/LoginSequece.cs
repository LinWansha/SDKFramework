namespace SDKFramework.Account
{
    public class LoginSequence
    {
        private enum ETaskName
        {
            START,
            CHOOSE_CHANNEL,
            GET_OAUTH,
            REQ_LOGIN_DATA,
            VALIDATION,
            WAITING
        }
        // ...
    }
}