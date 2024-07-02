namespace SDKFramework.Network
{
    public partial class NetworkModule : BaseModule
    {
        
        private string URL_USER_SERVER;

        protected internal override void OnModuleInit()
        {
            base.OnModuleInit();
            
            if (Global.IsDebug)
                URL_USER_SERVER = $"{Global.AccountServerURL.test}/api/v1/";
            else
                URL_USER_SERVER = $"{Global.AccountServerURL.prod}/api/v1/";
        }
    }
}