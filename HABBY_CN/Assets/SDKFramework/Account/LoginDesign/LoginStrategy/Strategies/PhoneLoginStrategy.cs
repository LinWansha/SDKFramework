using SDKFramework.Message;
using SDKFramework.UI;

namespace SDKFramework.LoginDesign
{
    public class PhoneLoginStrategy : LoginTemplate
    {
        public override void Login()
        {
            HLogger.Log("PhoneLogin");
            UIMediator loginMediator = HabbyFramework.UI.OpenUISingle(UIViewID.LoginUI);
            loginMediator.ViewObject.GetComponent<LoginView>().ActivateWindow(2);
            HabbyFramework.Message.Subscribe<MsgType.PhoneLogin>(_PhoneLogin);            
        }

        private void _PhoneLogin(MsgType.PhoneLogin arg)
        {
            //验证手机验证码 手机号登录
            HLogger.Log($"验证手机验证码 用手机号登录 手机号：[{arg.phoneNumber}] 验证码： [{arg.phoneVerifyCode}]");
        }
    }
}