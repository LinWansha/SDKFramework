using System;
using SDKFramework.Account.DataSrc;
using SDKFramework.Account.Net;

namespace SDKFramework.Account
{
    public class EditorLoginStrategy : LoginTemplate
    {
        protected override string Channel => UserAccount.ChannelEditor;

        public override void ChannelLogin(Action<LoginResponse> onResponse)
        {
            HabbyUserClient.Instance.LoginEditorChannel(onResponse, Channel, HabbyUserClient.Instance.DeviceId);
        }
    }
}