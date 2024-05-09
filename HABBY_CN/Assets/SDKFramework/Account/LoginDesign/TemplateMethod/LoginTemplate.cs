namespace SDKFramework.LoginDesign
{
    public abstract class LoginTemplate:ILoginStrategy
    {
        //... The login process requires the steps, needed to change the implementation in subclass overwrite

        // protected Func<bool> IsAgreePrivacy;


        public LoginTemplate()
        {
            // GraspPrivacyStatus();
        }

        // protected bool GraspPrivacyStatus(bool isAgree = false)
        // {
        //     IsAgreePrivacy = false; 
        //     return IsAgreePrivacy;
        // }


        public bool CheckPrivacyStatus()
        {
            //TODO:从用户持久化数据中拿取
            return HabbyFramework.Account.CurrentAccount.IsAgreePrivacy;
        }

        public abstract void Login();
    }
}
