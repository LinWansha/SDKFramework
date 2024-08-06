using System;
using SDKFramework.Account.DataSrc;
using SDKFramework.Account.Net;
using Sdkhubv2.Runtime.tools;

namespace SDKFramework.Account
{
    public class WxLoginStrategy : LoginTemplate
    {
        protected override string Channel => UserAccount.ChannelWeiXin;
        
        public override void ChannelLogin(Action<LoginResponse> onResponse)
        {
            WeChatAPIUtil.RequestLoginAuthToken((code, data) =>
            {
                HabbyFramework.Analytics.TGA_cn_login_result(LoginStepCN.ask_3rd_auth,code,$"weixin oauth result code:{code},msg:{data}");
                AccountLog.Info("#onRemoteQqLogin result code:" + code + " msg:" + data);
                if (code==0)
                {
                    HabbyUserClient.Instance.LoginWechat(onResponse,data,"");
                }
            },
            ((errorCode, errorMsg) =>
            {
                AccountLog.Warn($"RequestShanYanAuthToken onError ==  code:{errorCode},msg:{errorMsg}");
            }));
        }
    }
}