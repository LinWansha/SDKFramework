using System;

namespace Habby.Log
{
    public class UnityLogger: ILog
    {
        public void Debug(string msg)
        {
            UnityEngine.Debug.Log(msg);
        }

        public void Warning(string msg)
        {
            UnityEngine.Debug.LogWarning(msg);
        }
        public void Error(string msg)
        {
            UnityEngine.Debug.LogError(msg);
        }

        public void Exception(Exception exception)
        {
            UnityEngine.Debug.LogError(exception);
        }

        public void Error(Exception e)
        {
            UnityEngine.Debug.LogError(e);
        }

        public void Warning(string message, params object[] args)
        {
            UnityEngine.Debug.LogFormat(message, args);
        }

        public void Debug(string message, params object[] args)
        {
            UnityEngine.Debug.LogFormat(message, args);
        }

        public void Error(string message, params object[] args)
        {
            UnityEngine.Debug.LogErrorFormat(message, args);
        }

        public void Log(string message, string tag)
        {
            UnityEngine.Debug.Log($"[{tag}]{message}");
        }
    }
}
