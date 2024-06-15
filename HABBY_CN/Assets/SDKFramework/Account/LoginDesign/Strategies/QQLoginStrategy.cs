namespace SDKFramework.Account
{
    public class QQLoginStrategy : LoginTemplate
    {
        protected override string Channel => "QQ";
        
        public override void Login(RespHandler handler)
        {
            AccountLog.Info("QQLogin");
            // ...
            handler.failed();
        }
    }
}
