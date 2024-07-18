using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace Habby.Log
{
    public class RemoteLogger:ILog
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
            if (IsEnable)
            {
                sendRemote(msg,"debug");
            }
        }

        public void Warning(string msg)
        {
            if (IsEnable)
            {
                sendRemote(msg, "warning");
            }
        }
        public void Error(string msg)
        {
            if (IsEnable)
            {
                sendRemote(msg, "error");
            }
        }

        public void Exception(Exception exception)
        {
            if (IsEnable)
            {
                sendRemote(exception.ToString(),"exception");
            }
        }

        public void Error(Exception e)
        {
            if (IsEnable)
            {
                sendRemote(e.ToString(), "error");
            }
        }

        public void Warning(string message, params object[] args)
        {
            if (IsEnable)
            {
                sendRemote(string.Format(message, args), "warning");
            }
        }

        public void Debug(string message, params object[] args)
        {
            if (IsEnable)
            {
                sendRemote(string.Format(message, args), "debug");
            }
        }

        public void Error(string message, params object[] args)
        {
            if (IsEnable)
            {
                sendRemote(string.Format(message, args), "error");
            }
        }

        public void Log(string message, string tag)
        {
            if (IsEnable)
            {
                sendRemote(message, tag);
            }
        }

        private void sendRemote(string content,string tag)
        {
            string url =  $"{remoteURL}/log?msg={content}&tag={tag}";
            UnityWebRequest request = new UnityWebRequest(url, "POST");
            request.SendWebRequest();
        }
    }
}