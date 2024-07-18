using System;
using SDKFramework.Account.Net;


namespace SDKFramework.Account
{
    public class HistoryLoginStrategy : LoginTemplate
    {
        protected override string Channel => Global.Channel;
        public override void ChannelLogin(Action<LoginResponse> onResponse)
        {
            HabbyUserClient.Instance.LoginWithToken(onResponse,Global.Channel,HabbyFramework.Account.CurrentAccount.AccessToken);
        }
    }
}