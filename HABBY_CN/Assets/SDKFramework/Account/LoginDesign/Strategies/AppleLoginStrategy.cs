namespace SDKFramework.Account
{
    public class AppleLoginStrategy : LoginTemplate
    {
        protected override string Channel => "Apple";

        public override void Login(RespHandler handler)
        {
            AccountLog.Info("AppleLogin");
            handler.success();
            // ...
        }
    }
}