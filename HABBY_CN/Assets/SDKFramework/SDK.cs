using Newtonsoft.Json;
using SDKFramework.Config;
using UnityEngine;

namespace SDKFramework
{
    public static class Global
    {
        static Global()
        {
            App = ParseConfig<AppConfig>("AppConfig");

            var webConfig = ParseConfig<WebConfig>("WebConfig");
            WebView = webConfig.WebView;
            AccountServerURL = webConfig.AccountServerURL;

            Platform = Application.platform;
        }

        private static T ParseConfig<T>(string configName) where T : struct
        {
            string json = HabbyFramework.Asset.LoadConfig(configName);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static readonly AppConfig App;
    
        public static readonly RuntimePlatform Platform;

        public static readonly WebConfig.WebViewData WebView;

        public static readonly WebConfig.AccountServerData AccountServerURL;
        
        public static string Channel => HabbyFramework.Account.CurrentAccount?.LoginChannel;
    }
    
    public class SDK : MonoBehaviour
    {
        private static readonly GameObject TheChosenOne;

        static SDK()
        {
            TheChosenOne = GameObject.Find("SDK");

            if (TheChosenOne == null)
            {
                Log.Error("[You need to add SDK prefabrication to the Hierarchy before integration SDK]");
            }
        }

        public static SDK New()
        {
            // ReSharper disable once Unity.NoNullPropagation
            SDK Kernel = TheChosenOne?.AddComponent<SDK>();
            return Kernel;
        }

        public void Run()
        {
        }
        
    }
    
}