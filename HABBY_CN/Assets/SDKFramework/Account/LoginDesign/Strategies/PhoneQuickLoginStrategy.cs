using System;
using SDKFramework.Account.DataSrc;
using SDKFramework.Account.Net;
using Sdkhubv2.Runtime.tools;

namespace SDKFramework.Account
{
    public class PhoneQuickLoginStrategy: LoginTemplate
    {
        protected override string Channel => UserAccount.ChannelPhoneQuick;
        public override void ChannelLogin(Action<LoginResponse> onResponse)
        {
            AccountLog.Info("IsShanYanValid" + ShanYanUtil.IsShanYanValid());
            AccountLog.Info("hasSimeCard" + ShanYanUtil.hasSimeCard());
            AccountLog.Info("getState" + ShanYanUtil.getState());
            ShanYanUtil.RequestShanYanAuthToken((code, data) =>
            {
                AccountLog.Warn($"RequestShanYanAuthToken onResult ==  code:{code},msg:{data}");
                switch (code)
                {
                    case 0:
                        HabbyUserClient.Instance.LoginPhoneQuickChannel(onResponse, data);
                        break;
                    case 1012:                                  //手机一键登录隐私勾选打点
                    case 1013 when data == "1":
                        AccountLog.Info(data == "1" ? "RequestShanYanAuthToken 选中" : "RequestShanYanAuthToken 取消");
                        break;
                }
            },
            ((errorCode, errorMsg) =>
            {
                AccountLog.Warn($"RequestShanYanAuthToken onError ==  code:{errorCode},msg:{errorMsg}");
            }));
            
        }
    }
}