using System;
using Newtonsoft.Json;
using SDKFramework.Account.DataSrc;
using SDKFramework.Account.Net;
using Sdkhubv2.Runtime.tools;

namespace SDKFramework.Account
{
    public class QQLoginStrategy : LoginTemplate
    {
        protected override string Channel => UserAccount.ChannelQQ;
        
        public override void ChannelLogin(Action<LoginResponse> onResponse)
        {
            
            QQAPIUtil.RequestQqLoginAuthToken(
                (code, msg) =>
                {
                    AccountLog.Info("#onRemoteQqLogin result code:" + code + " msg:" + msg);
                    HabbyFramework.Analytics.TGA_cn_login_result(LoginStepCN.ask_3rd_auth,code,$"qq oauth result code:{code},msg:{msg}");
                    OAuthResult oauthData = JsonConvert.DeserializeObject<OAuthResult>(msg);
                    HabbyUserClient.Instance.LoginQQChannel(onResponse, oauthData.accessToken);
                }, (code, msg) =>
                {
                    AccountLog.Warn("#onRemoteQqLogin error code:" + code + " msg:" + msg);
                }
            );
            
        }
        
        public class OAuthResult
        {
            public int ret;
            public string accessToken;
            public string expires_in;
            public string openid;
        }
    }
    
}
