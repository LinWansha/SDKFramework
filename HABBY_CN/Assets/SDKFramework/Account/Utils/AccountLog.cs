using UnityEngine;

namespace SDKFramework.Account
{
    public class AccountLog
    {
        static readonly string TAG = "[Account]";

        static readonly Color Color = Color.cyan;

        public static void Info(object message)
        {
            Log.Info(message, TAG.DoMagic(Color));
        }

        public static void Warn(object message)
        {
            Log.Warn(message, TAG.DoMagic(Color));
        }

        public static void Error(object message)
        {
            Log.Error(message, TAG.DoMagic(Color));
        }
    }
}