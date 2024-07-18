using Sdkhubv2.Runtime;
using UnityEngine;

namespace Habby.Account
{

    public class SDKHubLog
    {
        public enum DLogType
        {
            Log = 1,
            Warning,
            Error,
            Assert,
            TrueLog,
        }

        public static string LogTag = "[SDKHub]";

        public static DLogType MinLogType = DLogType.Log;
        private static bool IsShow(DLogType type)
        {
            int ret = (int)type - (int)MinLogType;
            if (ret < 0) return false;
            return true;
        }

        public static void TagLog(DLogType logType, string tag, object pObject)
        {
            if (!IsShow(DLogType.Log)) return;

            OutputLog(logType, $"[{tag}]{pObject}");
        }

        public static void TagLogFormat(DLogType logType, string tag, string format, params object[] paramObjs)
        {
            if (!IsShow(DLogType.Log)) return;

            OutputLog(logType, $"[{tag}]{string.Format(format, paramObjs)}");
        }

        public static void Log(object _object)
        {
            OutputString(DLogType.Log, _object);
        }

        public static void LogWarning(object _object)
        {
            OutputString(DLogType.Warning, _object);
        }

        public static void LogError(object _object)
        {
            OutputString(DLogType.Error, _object);
        }

        public static void LogAssertion(object _object)
        {
            OutputString(DLogType.Assert, _object);
        }

        public static void LogException(string msg, System.Exception error)
        {
            if (!IsShow(DLogType.Error)) return;
            try
            {
                Log(msg);
                LogErrorFormat("[Exception]{0}", error);
            }
            catch (System.Exception e)
            {
                LogError(e);
            }
        }

        public static void LogFormat(string format, params object[] args)
        {
            OutputFormatString(DLogType.Log, format, args);
        }

        public static void LogWarningFormat(string format, params object[] args)
        {
            OutputFormatString(DLogType.Warning, format, args);
        }

        public static void LogErrorFormat(string format, params object[] args)
        {
            OutputFormatString(DLogType.Error, format, args);
        }


        private static void OutputString(DLogType type, object tar)
        {
            try
            {
                if (!IsShow(type)) return;
                OutputLog(type, tar == null ? "Null" : tar.ToString());
            }
            catch (System.Exception ex)
            {
                Debug.LogError(tar?.ToString());
                Debug.LogError(ex);
            }
        }

        private static void OutputFormatString(DLogType type, string format, params object[] args)
        {
            try
            {
                if (!IsShow(type)) return;
                string msg = null;
                if (args.Length == 1)
                {
                    msg = string.Format(format, args[0]);
                }
                else if (args.Length == 2)
                {
                    msg = string.Format(format, args[0], args[1]);
                }
                else if (args.Length == 3)
                {
                    msg = string.Format(format, args[0], args[1], args[2]);
                }
                else
                {
                    msg = string.Format(format, args);
                }
 
                OutputLog(type, msg);
            }
            catch (System.Exception ex)
            {
                Debug.LogError(format);
                Debug.LogError(ex);
            }
        }

        private static void OutputLog(DLogType type, string msg)
        {
            var tmsg = $"{LogTag}[Tick:{System.DateTime.Now.TimeOfDay}]: {msg} ";
            HabbySDKHubManager.Instance?.PlatformBridge?.Log(tmsg);
            #if UNITY_EDITOR
            // ENABLE_DEBUG && 
            switch (type)
            {
                case DLogType.TrueLog:
                case DLogType.Log:
                    UnityEngine.Debug.Log(tmsg);
                    break;
                case DLogType.Error:
                    UnityEngine.Debug.LogError(tmsg);
                    break;
                case DLogType.Warning:
                    UnityEngine.Debug.LogWarning(tmsg);
                    break;
                case DLogType.Assert:
                    UnityEngine.Debug.LogAssertion(tmsg);
                    break;
                default:
                    break;
            }
            #endif
        }
    }
}



