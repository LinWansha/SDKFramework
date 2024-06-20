using System;
using SDKFramework.Account.DataSrc;
using SDKFramework.Account.Net;

namespace SDKFramework.Account
{
    public class WxLoginStrategy : LoginTemplate
    {
        protected override string Channel => UserAccount.ChannelWeiXin;
        
        public override void ChannelLogin(Action<LoginResponse> onResponse)
        {
            HabbyUserClient.Instance.LoginWechat(onResponse,"","");
        }
    }
}