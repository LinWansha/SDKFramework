using System;
using SDKFramework.Account.DataSrc;
using SDKFramework.Account.Net;

namespace SDKFramework.Account
{
    public class AppleLoginStrategy : LoginTemplate
    {
        protected override string Channel => UserAccount.ChannelAppleId;

        public override void ChannelLogin(Action<LoginResponse> onResponse)
        {
            HabbyUserClient.Instance.LoginAppleId(onResponse,"","",null,"");
        }
    }
}