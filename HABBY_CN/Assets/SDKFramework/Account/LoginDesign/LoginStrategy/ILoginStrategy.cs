namespace SDKFramework.Account
{
    public interface ILoginStrategy
    {
        bool CheckPrivacyStatus();
        void Login();
    }

}






