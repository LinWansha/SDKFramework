using System;
using SDKFramework.Config;
using SDKFramework.Message;
using UnityEngine;

namespace SDKFramework
{
    public class SDK : MonoBehaviour
    {
        private static readonly GameObject TheChosenOne;

        public static ProcedureOption Procedure;

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
            var appJson = HabbyFramework.Asset.LoadConfig("AppConfig");
            var webJson = HabbyFramework.Asset.LoadConfig("WebConfig");
            var AppData = JsonUtility.FromJson<AppConfig>(appJson);
            var webView = JsonUtility.FromJson<WebConfig>(webJson);
            HabbyFramework.Message.Post(AppData);
            HabbyFramework.Message.Post(webView);
            // ReSharper disable once Unity.NoNullPropagation
            SDK Kernel = TheChosenOne?.AddComponent<SDK>();
            return Kernel;
        }

        public void Run(ProcedureOption option)
        {
            Procedure = option;
            option.Splash?.Invoke();
        }

        public class ProcedureOption
        {
            public Action Splash;

            public Action Login;

            public Action EnterGame;
        }

        private void Update()
        {
            //TODO:Anything...
        }
    }

    public class AppSource : MessageHandler<AppConfig>
    {
        public static AppConfig Config;
        
        public static WebConfig WebView;

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
}