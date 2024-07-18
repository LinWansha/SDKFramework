using System;
using SDKFramework.Account.DataSrc;
using SDKFramework.Account.Net;
using SDKFramework.Message;
using SDKFramework.UI;

namespace SDKFramework.Account
{

    public struct PhoneInfo
    {
        public string phoneNumber;
        public string verifyCode;
    }
    class PhoneLoginInfo:MessageHandler<PhoneInfo>
    {
        public static string phoneNumber;
        public static string verifyCode;
        public override void HandleMessage(PhoneInfo arg)
        {
            verifyCode = arg.verifyCode;
            phoneNumber = arg.phoneNumber;
        }
    }
    public class PhoneLoginStrategy : LoginTemplate
    {
        protected override string Channel => UserAccount.ChannelPhone;

        public override void ChannelLogin(Action<LoginResponse> onResponse)
        {
            AccountLog.Info($"验证手机验证码 用手机号登录 手机号：[{PhoneLoginInfo.phoneNumber}] 验证码： [{PhoneLoginInfo.verifyCode}]");
            HabbyUserClient.Instance.LoginPhoneChannel(onResponse,PhoneLoginInfo.phoneNumber,PhoneLoginInfo.verifyCode);
        }
        
    }
}