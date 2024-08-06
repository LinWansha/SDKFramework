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
                HabbyFramework.Analytics.TGA_cn_login_result(LoginStepCN.ask_3rd_auth,code,$"shanyan oauth result code:{code},msg:{data}");
                switch (code)
                {
                    case 0:
                        HabbyUserClient.Instance.LoginPhoneQuickChannel(onResponse, data);
                        break;
                    case 1011:
                        HabbyFramework.UI.OpenUISingle(UIViewID.LoginUI);
                        break;
                    case 1012:
                        bool isAgree = data != "0";
                        HabbyFramework.Account.SetPrivacyStatus(isAgree);
                        break;
                    case 1013:
                        AccountLog.Info("click phone quick login");
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