using System;
using System.Net;
using UnityEngine.Networking;

namespace Habby.Log
{
    public class UnityAndRemoteLogger:ILog
    {
        public string remoteURL = "";

        private bool IsEnable
        {
            get
            {
                return !string.IsNullOrEmpty(remoteURL);
            }
        }
        public void Debug(string msg)
        {
            UnityEngine.Debug.Log(msg);
            if (IsEnable)
            {
                sendRemote(msg,"debug");
            }
        }

        public void Warning(string msg)
        {
            UnityEngine.Debug.LogWarning(msg);
            if (IsEnable)
            {
                sendRemote(msg, "warning");
            }
        }
        public void Error(string msg)
        {
            UnityEngine.Debug.LogError(msg);
            if (IsEnable)
            {
                sendRemote(msg, "error");
            }
        }

        public void Exception(Exception exception)
        {
            UnityEngine.Debug.LogError(exception);
            if (IsEnable)
            {
                sendRemote(exception.ToString(),"exception");
            }
        }

        public void Error(Exception e)
        {
            UnityEngine.Debug.LogError(e);
            if (IsEnable)
            {
                sendRemote(e.ToString(), "error");
            }
        }

        public void Warning(string message, params object[] args)
        {
            UnityEngine.Debug.LogFormat(message, args);
            if (IsEnable)
            {
                sendRemote(string.Format(message, args), "warning");
            }
        }

        public void Debug(string message, params object[] args)
        {
            UnityEngine.Debug.LogFormat(message, args);
            if (IsEnable)
            {
                sendRemote(string.Format(message, args), "debug");
            }
        }

        public void Error(string message, params object[] args)
        {
            UnityEngine.Debug.LogErrorFormat(message, args);
            if (IsEnable)
            {
                sendRemote(string.Format(message, args), "error");
            }
        }

        public void Log(string message, string tag)
        {
            UnityEngine.Debug.Log(message);
            if (IsEnable)
            {
                sendRemote(message,tag);
            }
        }

        private void sendRemote(string content,string tag)
        {
            string encodedContent = WebUtility.UrlEncode(content);
            string url =  $"{remoteURL}/log?msg={encodedContent}&tag={tag}";
            UnityWebRequest request = new UnityWebRequest(url, "POST");
            request.SendWebRequest();
        }
    }
}