using System;
using SDKFramework.Account.Net;

namespace SDKFramework.Account
{
    public class WxLoginStrategy : LoginTemplate
    {
        protected override string Channel => "Wx";
        
        public override void ChannelLogin(Action<LoginResponse> onResponse)
        {
            HabbyUserClient.Instance.LoginWechat(onResponse,"","");
        }
    }
}