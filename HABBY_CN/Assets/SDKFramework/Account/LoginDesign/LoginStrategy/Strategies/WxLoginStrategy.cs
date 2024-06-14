namespace SDKFramework.Account
{
    public class WxLoginStrategy : LoginTemplate
    {
        public override void Login()
        {
            AccountLog.Info("WxLogin");
            // ...
        }
    }
}