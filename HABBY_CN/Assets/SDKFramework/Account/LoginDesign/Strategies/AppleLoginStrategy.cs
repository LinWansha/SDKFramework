using System;
using SDKFramework.Account.Net;

namespace SDKFramework.Account
{
    public class AppleLoginStrategy : LoginTemplate
    {
        protected override string Channel => "Apple";

        public override void ChannelLogin(Action<LoginResponse> onResponse)
        {
            HabbyUserClient.Instance.LoginAppleId(onResponse,"","",null,"");
        }
    }
}