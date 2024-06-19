using System;
using SDKFramework.Account.Net;

namespace SDKFramework.Account
{
    public class QQLoginStrategy : LoginTemplate
    {
        protected override string Channel => "QQ";
        
        public override void ChannelLogin(Action<LoginResponse> onResponse)
        {
            string token = "";
            HabbyUserClient.Instance.LoginQQChannel(onResponse, token);
        }
    }
}
