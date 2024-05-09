namespace SDKFramework.LoginDesign
{
    public class WxLoginStrategy : LoginTemplate
    {
        public override void Login()
        {
            HLogger.Log("WxLogin");
            // ...
        }
    }
}