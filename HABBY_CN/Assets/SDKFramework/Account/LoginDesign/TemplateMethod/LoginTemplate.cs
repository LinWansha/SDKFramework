using System;
using SDKFramework.Account.Net;

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
            AccountLog.Info($"{Channel}Login Start");
            ChannelLogin((response) =>
            {
                if (LoginResponse.CODE_SUCCESS == response.code)
                {
                    OnLoginSuccess(handler);
                }
                else
                {
                    OnLoginFailed(handler);
                }

                AccountLog.Info(response.code);
            });
        }

        private void OnLoginFailed(RespHandler handler)
        {
            AccountLog.Info($"{Channel} µÇÂ¼Ê§°Ü");
            
            handler.failed();
        }

        private void OnLoginSuccess(RespHandler handler)
        {
            AccountLog.Info($"{Channel} µÇÂ¼³É¹¦");
            
            handler.success();
        }

        public abstract void ChannelLogin(Action<LoginResponse> onResponse);

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
