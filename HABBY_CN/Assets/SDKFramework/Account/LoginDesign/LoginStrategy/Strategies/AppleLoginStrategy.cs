namespace SDKFramework.LoginDesign
{
    public class AppleLoginStrategy : LoginTemplate
    {
        public override void Login()
        {
            HLogger.Log("AppleLogin");
            // ...
        }
    }
}