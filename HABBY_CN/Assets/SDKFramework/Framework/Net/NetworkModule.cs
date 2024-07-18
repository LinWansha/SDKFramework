namespace SDKFramework.Network
{
    class AccountConfig
    {
        public string Url;
        public string netVersion;
        public string accountType;
    }
    class ServerConfig
    {
        public string profile;
    
        public string overridable;
    }
    public partial class NetworkModule : BaseModule
    {
        
        private string URL_USER_SERVER;

        protected internal override void OnModuleInit()
        {
            base.OnModuleInit();


            // var json = Resources.Load<TextAsset>("ServerConfig");
            // var serverConfig = JsonConvert.DeserializeObject<ServerConfig>(json.text);
            // var accountJson = Resources.Load<TextAsset>($"{serverConfig.profile}/HabbyAccountConfig");
            // var data = JsonConvert.DeserializeObject<AccountConfig>(accountJson.text);
            //
            // URL_USER_SERVER = $"{data.Url}/api/v1/";
            
            if (Global.IsDebug)
                URL_USER_SERVER = $"{Global.AccountServerURL.test}/api/v1/";
            else
                URL_USER_SERVER = $"{Global.AccountServerURL.prod}/api/v1/";
        }
    }
}