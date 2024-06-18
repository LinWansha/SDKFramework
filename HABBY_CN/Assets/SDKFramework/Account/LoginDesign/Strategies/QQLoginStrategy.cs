using SDKFramework.Account.Net;

namespace SDKFramework.Account
{
    public class QQLoginStrategy : LoginTemplate
    {
        protected override string Channel => "QQ";
        
        public override void Login(RespHandler handler)
        {
            AccountLog.Info("QQLogin");
            // ...
            string token = "";
            HabbyUserClient.Instance.LoginQQChannel((response) =>
            {
                if (LoginResponse.CODE_SUCCESS == response.code)
                {
                    handler.success();
                    AccountLog.Info("qq登录成功");
                }
                else
                {
                    handler.failed();

                    AccountLog.Info("qq登录失败");
                }

                AccountLog.Info(response.code);
            }, token);
        }
    }
}
