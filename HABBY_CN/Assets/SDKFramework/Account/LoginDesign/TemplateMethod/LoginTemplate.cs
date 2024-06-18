using System;

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

        public abstract void Login(RespHandler handler);

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
