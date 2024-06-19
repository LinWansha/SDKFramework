using System;
using SDKFramework.Account.Net;
using SDKFramework.Message;
using SDKFramework.UI;

namespace SDKFramework.Account
{
    public class PhoneLoginStrategy : LoginTemplate
    {
        private LoginView _loginView;
        protected override string Channel => "Phone";

        private Action<LoginResponse> _onResponse;
        
        public override void ChannelLogin(Action<LoginResponse> onResponse)
        {
            _onResponse = onResponse;
            UIMediator loginMediator = HabbyFramework.UI.OpenUISingle(UIViewID.LoginUI);
            _loginView=loginMediator.ViewObject.GetComponent<LoginView>();
            _loginView.ActivateWindow(2);
            HabbyFramework.Message.Subscribe<SDKEvent.SendPhoneVerifyCode>(_SendPhoneVerifyCode);            
            HabbyFramework.Message.Subscribe<SDKEvent.PhoneLogin>(_PhoneLogin);            
        }

        private void _SendPhoneVerifyCode(SDKEvent.SendPhoneVerifyCode arg)
        {
            HabbyUserClient.Instance.RequestSmsCode(arg.phoneNumber,(response =>
            {
                if (response.code == 0 )
                {
                    AccountLog.Info("发送验证码 成功");
                    _loginView.ActivateWindow(3);
                }
                else if(response.code == SendUserSmsCodeResponse.CAPTCHA_EXCEEDED_TIMES) // 超次数
                {
                    AccountLog.Info("发送验证码 超次数");
                }
                else
                {
                    AccountLog.Info("发送验证码 失败"+response.code);
                }
            }));
        }

        private void _PhoneLogin(SDKEvent.PhoneLogin arg)
        {
            //验证手机验证码 手机号登录
            AccountLog.Info($"验证手机验证码 用手机号登录 手机号：[{arg.phoneNumber}] 验证码： [{arg.phoneVerifyCode}]");

            HabbyUserClient.Instance.LoginPhoneChannel(_onResponse,arg.phoneNumber,arg.phoneVerifyCode);
            
        }
    }
}