namespace SDKFramework.Account
{
    public class AppleLoginStrategy : LoginTemplate
    {
        public override void Login()
        {
            AccountLog.Info("AppleLogin");
            // ...
        }
    }
}