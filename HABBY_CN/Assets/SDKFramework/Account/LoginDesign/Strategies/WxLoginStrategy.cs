namespace SDKFramework.Account
{
    public class WxLoginStrategy : LoginTemplate
    {
        protected override string Channel => "Wx";
        
        public override void Login(RespHandler handler)
        {
            AccountLog.Info("WxLogin");
            // ...
        }
    }
}