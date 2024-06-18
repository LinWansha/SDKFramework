using SDKFramework.Account.Net;
using SDKFramework.Message;
using SDKFramework.UI;

namespace SDKFramework.Account
{
    public class PhoneLoginStrategy : LoginTemplate
    {
        private RespHandler _handler;
        
        protected override string Channel => "Phone";
        
        public override void Login(RespHandler handler)
        {
            _handler = handler;
            AccountLog.Info("PhoneLogin");
            UIMediator loginMediator = HabbyFramework.UI.OpenUISingle(UIViewID.LoginUI);
            loginMediator.ViewObject.GetComponent<LoginView>().ActivateWindow(2);
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

            HabbyUserClient.Instance.LoginPhoneChannel((resp) =>
            {
                if (resp.code==LoginResponse.CODE_SUCCESS)
                {
                    AccountLog.Info("手机登录 成功");
                }
                else
                {
                    AccountLog.Error($"手机登录 resp.code == {resp.code}");
                }
            },arg.phoneNumber,arg.phoneVerifyCode);
            
            
        }
    }
}