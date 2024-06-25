using System;
using SDKFramework.Account.DataSrc;
using SDKFramework.Account.Net;
using SDKFramework.Account.Utils;

namespace SDKFramework.Account
{
    public abstract class LoginTemplate:ILoginStrategy
    {
        //... The login process requires the steps, needed to change the implementation in subclass overwrite
    
        protected abstract string Channel { get; }

        public bool CheckPrivacyStatus()
        {
            return HabbyFramework.Account.CurrentAccount.IsAgreePrivacy;
        }
        
        private Action<LoginResponse> loginResponseHandler;
        internal void Login(RespHandler handler)
        {
            AccountLog.Info($"{Channel} Login Start");
            loginResponseHandler = (response) =>
            {
                if (Response.CODE_SUCCESS == response.code)
                {
                    OnLoginSuccess(response);
                    handler.success();
                }
                else
                {
                    OnLoginFailed(response);
                    handler.failed();
                }
            };
            if (!HabbyFramework.Account.HasAccount)
            {
                ChannelLogin(loginResponseHandler);
            }
            else
            {
                AccountLog.Info("LoginWithToken");
                HabbyUserClient.Instance.LoginWithToken(loginResponseHandler,Channel, HabbyFramework.Account.CurrentAccount.AccessToken);
            }
        }
        
        public abstract void ChannelLogin(Action<LoginResponse> onResponse);

        private void OnLoginSuccess(LoginResponse response)
        {
            AccountLog.Info($"{Channel} 登录成功");
            
            HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.get_data_success);
            HabbyFramework.UI.CloseUI(UIViewID.LoginUI);
            
            AccountDataUtil.ParseAndSaveAccount(response,Channel);
        }

        private void OnLoginFailed(LoginResponse response)
        {
            AccountLog.Info($"{Channel} 登录失败, errorCode: {response.code}");
            switch (response.code)
            {
                case Response.CODE_APP_TOKEN_EXPIRE:
                    AccountLog.Warn($"{Channel} 授权过期");
                    break;
                case Response.CAPTCHA_INVALID:
                    AccountLog.Warn($"手机验证码错误");
                    break;
            }

            if (HabbyFramework.Account.HasAccount)
            {
                AccountLog.Info($"重新授权 {Channel}");
                ChannelLogin(loginResponseHandler);
            }
        }

        public void ValidateIdentity(RespHandler handler)
        {
            Action<bool, int> onComplete = null;
            onComplete = (success, code) =>
            {
                if (success)
                    handler.success();
                else
                    handler.failed();
                AccountModule.OnValidateIdentityResult -= onComplete;
            };
            HabbyFramework.Account.StartValidation(onComplete);
        }
        
        public void RealNameLogin(RespHandler handler)
        {
            HabbyFramework.Account.RealNameLogin((success) =>
            {
                if (success)
                    handler.success();
                else
                    handler.failed();
            });
        }
    }

    public struct RespHandler
    {
        public Action success;

        public Action failed;
    }
}
