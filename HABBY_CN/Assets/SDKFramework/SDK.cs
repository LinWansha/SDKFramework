using System;
using SDKFramework.Config;
using SDKFramework.Message;
using UnityEngine;

namespace SDKFramework
{
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
            static void PostConfig<T>(string configName) where T : struct
            {
                string json = HabbyFramework.Asset.LoadConfig(configName);
                T config = JsonUtility.FromJson<T>(json);
                HabbyFramework.Message.Post(config);
            }
            PostConfig<AppConfig>("AppConfig");
            PostConfig<WebConfig>("WebConfig");
            // ReSharper disable once Unity.NoNullPropagation
            SDK Kernel = TheChosenOne?.AddComponent<SDK>();
            return Kernel;
        }

        public void Run()
        {
        }
        

        private void Update()
        {
            //TODO:Anything...
        }
    }
    

    public class AppSource : MessageHandler<AppConfig>
    {
        public static AppConfig Config;
        
        public static readonly RuntimePlatform Platform;

        static AppSource()
        {
            Platform= Application.platform;
        }
        public override void HandleMessage(AppConfig arg)
        {
            Config = arg;
        }
    }
    
    
    public class WebSource : MessageHandler<WebConfig>
    {
        public static WebConfig Config;
        
        public override void HandleMessage(WebConfig arg)
        {
            Config = arg;
        }
    }
}