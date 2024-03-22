using System;
using SDKFramework.Config;
using SDKFramework.Message;
using UnityEngine;

namespace SDKFramework
{
    public class SDK : MonoBehaviour
    {
        private static readonly GameObject TheChosenOne;
        
        public static Action Login;

        public static Action EnterGame;
        
        public static Action Logout;
        
        static SDK()
        {
            TheChosenOne = GameObject.Find("SDK");

            if (TheChosenOne == null)
            {
                HLogger.LogError(
                    "接入之前请先在Hierarchy中放入SDK预制体 [You need to add SDK prefabrication to the Hierarchy before integration SDK]");
            }
        }

        public static SDK New()
        {
            var jsonStr = Resources.Load<TextAsset>("SDKConfig/App").text;
            var AppData = JsonUtility.FromJson<AppConfig>(jsonStr);
            HabbyFramework.Message.Post(AppData);
            // ReSharper disable once Unity.NoNullPropagation
            SDK Kernel = TheChosenOne?.AddComponent<SDK>();
            return Kernel;
        }

        public void Run(ProcedureOption option)
        {
            option.Splash?.Invoke();
            Login = option.Login;
            EnterGame = option.EnterGame;
            Logout = option.Logout;
        }

        public class ProcedureOption
        {
            public Action Splash;

            public Action Login;

            public Action EnterGame;

            public Action Logout;
        }

        private void Update()
        {
            //TODO:性能监测工具...
        }
    }

    public class AppSource : MessageHandler<AppConfig>
    {
        public static AppConfig Data;

        public override void HandleMessage(AppConfig arg)
        {
            Data = arg;
        }
    }
}