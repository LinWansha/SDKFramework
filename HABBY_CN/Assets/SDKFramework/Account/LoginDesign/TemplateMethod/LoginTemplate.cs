using System;
using SDKFramework.Account.DataSrc;
using SDKFramework.Account.Net;
using SDKFramework.Account.Utils;
using SDKFramework.Message;
using SDKFramework.Utils;

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

            try
            {
                ChannelLogin(loginResponseHandler);
                track_3rd_oauth(1);
            }
            catch (Exception e)
            {
                AccountLog.Info($"{Global.Channel} login exception :" + e.Message);
                HabbyFramework.Analytics.TGA_cn_login_result(LoginStepCN.choose_channel, ErrorCode.LOGIN_EXCEPTION, e.Message);
            }
        }

        /// <param name="action">1: ask ,2: successful 3: failure</param>
        private void track_3rd_oauth(int action)
        {
            if (Channel is
                not (UserAccount.ChannelQQ
                or UserAccount.ChannelPhoneQuick
                or UserAccount.ChannelWeiXin
                or UserAccount.ChannelAppleId)) return;
            
            if (action == 1)
            {
                HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.ask_3rd_auth);
            }

            if (action == 2)
            {
                HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.ask_3rd_auth_success);
            }

            if (action == 3)
            {
                HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.ask_3rd_auth_fail);
            }
        }

        public abstract void ChannelLogin(Action<LoginResponse> onResponse);

        private void OnLoginSuccess(LoginResponse response)
        {
            AccountLog.Info($"{Channel} login successful");
            
            track_3rd_oauth(2);
            
            if (Channel == UserAccount.ChannelPhone)
            {
                HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.code_success);
            }
            
            HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.get_data_success);
            HabbyFramework.UI.CloseUI(UIViewID.LoginUI);
            
            AccountDataUtil.ParseUserAccount(response,Channel);

            if (Global.IsEditor)
            {
                HabbyFramework.Message.Post(new SDKEvent.SDKLoginFinish()
                {
                    code = response.code, msg = $"{Channel} login success", 
                });
            }
        }

        private void OnLoginFailed(LoginResponse response)
        {
            AccountLog.Info($"{Channel} login failure, errorCode: {response.code}");
            
            track_3rd_oauth(3);
            HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.choose_channel_fail);
            HabbyFramework.Analytics.TGA_cn_login_result(LoginStepCN.get_data_success,response.code,$"{Channel} login failure, errorCode: {response.code}");

            switch (response.code)
            {
                case Response.CODE_APP_TOKEN_EXPIRE:
                    HabbyTextHelper.Instance.ShowTip(string.Format(ErrorMessage.OAUTH_EXPIRE,Channel));
                    HabbyFramework.UI.OpenUISingle(UIViewID.LoginUI);
                    break;
                case Response.CAPTCHA_INVALID:
                    HabbyTextHelper.Instance.ShowTip(ErrorMessage.SMS_VERIFY_CODE_ERROR);
                    break;
                case Response.CODE_USER_NOT_FOUND:
                    HabbyTextHelper.Instance.ShowTip(ErrorMessage.USER_NOT_FOUND);
                    HabbyFramework.UI.OpenUISingle(UIViewID.LoginUI);
                    break;
                default:
                    HabbyTextHelper.Instance.ShowTip(string.Format(ErrorMessage.LOGIN_FAILURE,Channel));
                    break;
            }
            HabbyFramework.Message.Post(new SDKEvent.SDKLoginFinish()
            {
                code = response.code,
                msg = $"{Channel} login failed, errorCode: {response.code}",
            });

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
