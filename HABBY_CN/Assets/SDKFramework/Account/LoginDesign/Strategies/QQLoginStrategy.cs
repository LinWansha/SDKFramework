using System;
using Newtonsoft.Json;
using SDKFramework.Account.DataSrc;
using SDKFramework.Account.Net;
using Sdkhubv2.Runtime.tools;
using UnityEngine;

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
                    Debug.Log("#onRemoteQqLogin result code:" + code + " msg:" + msg);
                    OAuthResult oauthData = JsonConvert.DeserializeObject<OAuthResult>(msg);
                    HabbyUserClient.Instance.LoginQQChannel(onResponse, oauthData.accessToken);
                }, (code, msg) =>
                {
                    Debug.Log("#onRemoteQqLogin error code:" + code + " msg:" + msg);
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
