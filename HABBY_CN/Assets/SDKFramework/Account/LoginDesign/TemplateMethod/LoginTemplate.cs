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
            HabbyFramework.Account.LoginOrIdentify((success) =>
            {
                if (success)
                {
                    
                }
            });
        }
    }

    public class RespHandler
    {
        public Action success;

        public Action failed;
    }
}
