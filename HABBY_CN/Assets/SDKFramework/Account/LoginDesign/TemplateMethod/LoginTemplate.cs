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

        protected LoginTemplate()
        {
            // GraspPrivacyStatus();
        }
        

        public bool CheckPrivacyStatus()
        {
            return HabbyFramework.Account.CurrentAccount.IsAgreePrivacy;
        }
        
        internal void Login(RespHandler handler)
        {
            AccountLog.Info($"{Channel} Login Start");
            Action<LoginResponse> loginResponseHandler = (response) =>
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
                Log.Info("LoginWithToken");
                HabbyUserClient.Instance.LoginWithToken(loginResponseHandler,Channel, HabbyFramework.Account.CurrentAccount.AccessToken);
            }
           
        }
        
        public abstract void ChannelLogin(Action<LoginResponse> onResponse);

        private void OnLoginSuccess(LoginResponse response)
        {
            AccountLog.Info($"{Channel} 登录成功");
            HabbyFramework.UI.CloseUI(UIViewID.LoginUI);
            HabbyFramework.UI.CloseUI(UIViewID.QuickLoginUI);
            UserAccount account = AccountDataUtil.ParseLoginAccountInfo(response);
            account.UID = response.data.userId;
            account.LoginChannel = Channel;
            HabbyFramework.Account.Save(account);
            if (response.data.isNewUser && true)
            {
                Log.Info("IsNewUSer");
                //todo: track_first_active
            }
            else
            {
                Log.Info("IsNotNewUSer");
                //todo: track_not_new_user
            }
            //HabbyCloudConfigManager.Instance.SetGmUserId(account.UID);
            
        }

        private void OnLoginFailed(LoginResponse response)
        {
            AccountLog.Info($"{Channel} 登录失败");
            switch (response.code)
            {
                case Response.CODE_APP_TOKEN_EXPIRE:
                    AccountLog.Warn($"{Channel} 授权过期");
                    break;
                case Response.CAPTCHA_INVALID:
                    AccountLog.Warn($"手机验证码错误");
                    break;
                default:
                    // HabbyFramework.Account.ClearCurrent();
                    break;
            }
        }

        public void ValidateIdentity(RespHandler handler)
        {
            HabbyFramework.Account.StartValidation((success,code) =>
            {
                if (success)
                    handler.success();
                else
                    handler.failed();
            });
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
