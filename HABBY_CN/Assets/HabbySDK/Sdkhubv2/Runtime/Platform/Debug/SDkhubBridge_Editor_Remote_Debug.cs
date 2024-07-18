using System;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_REMOTE_DEBUG_SDK
using System.IO;
using BestHTTP.WebSocket;
using Habby;
using Habby.Events;
using Newtonsoft.Json;
#endif


namespace Sdkhubv2.Runtime.Platform
{
    
    public class SDkhubBridge_Editor_Remote_Debug:IHabbySDKAPI
    {
        
        [System.Serializable]
        protected struct DebugConfig
        {
            public string serverUrl;
        }
     
#if ENABLE_REMOTE_DEBUG_SDK
        public static readonly string ConfigPath = $"Assets/HabbySDK/Sdkhubv2/Editor/DebugConfig.json";
        
        public string GetServerUrl()
        {
            if (!File.Exists(ConfigPath))
            {
                Debug.LogWarning("Configuration file not found! Please ensure the file exists at: " + ConfigPath);
                throw new Exception("Configuration file not found! Please ensure the file exists at: " + ConfigPath);
            }

            string json = File.ReadAllText(ConfigPath);
            Debug.Log("#json:"+json);
            DebugConfig config = JsonConvert.DeserializeObject<DebugConfig>(json);
            Debug.Log("#json server url:"+config.serverUrl);
            return config.serverUrl;
        }

        #region socket
        protected class RequestData
        {
            public Action<int,string> onError;
            public Action<int,string> onResult;
            public uint requestId;
        }
        
        protected class SocketData
        {
            public string action;
            public string msg;
            public int code;
            public uint requestId;
            public string api;
        }

        private WebSocket webSocket;
        
        private uint mRequestID;
        private readonly uint MIN_REQUEST_ID = 100;
        private Dictionary<uint,RequestData> mRequestDatas;
            
        void OnWebSocketOpen(WebSocket ws)
        {
            Debug.Log("WebSocket Opened");
        }
        void OnReceivedWebSocketMessage(WebSocket ws, string message)
        {
            Debug.Log("#Message received from server: " + message);
            SocketData data =  JsonConvert.DeserializeObject<SocketData>(message);
            
        
            if(data.action == null)
            {
                Debug.LogError("action is null");
                return;
            }
            if(data.action == "push")
            {
                Debug.Log("push msg:"+data.msg);
                HabbyEventManagerV1.Instance.Dispatch(data.api,data.msg);
                return;
            }
            else
            {
                if(data.requestId < MIN_REQUEST_ID)
                {
                    Debug.LogError("requestId is invalid");
                    return;
                }
            
                if(!mRequestDatas.ContainsKey(data.requestId))
                {
                    Debug.LogError("requestId is not exist:"+data.requestId);
                    return;
                }
                if(data.action == "onResult")
                {
                    mRequestDatas[data.requestId].onResult(data.code,data.msg);
                }
                else 
                {
                    mRequestDatas[data.requestId].onError(data.code,data.msg);
                }
            }
        }

        void OnWebSocketError(WebSocket ws, string error)
        {
            Debug.LogError("WebSocket Error: " + error);
        }

        void OnWebSocketClosed(WebSocket ws, ushort code, string message)
        {
            Debug.Log("WebSocket Closed: " + message);
        }
        #endregion
#endif
        
       

        public void OnCreate()
        {
#if ENABLE_REMOTE_DEBUG_SDK
            Debug.Log("#WebSocket start" );
            mRequestDatas = new Dictionary<uint, RequestData>();
            mRequestID = MIN_REQUEST_ID;
            webSocket = new WebSocket(new System.Uri(GetServerUrl()));
            webSocket.OnOpen += OnWebSocketOpen;
            webSocket.OnMessage += OnReceivedWebSocketMessage;
            webSocket.OnError += OnWebSocketError;
            webSocket.OnClosed += OnWebSocketClosed;
            webSocket.Open();
#endif
        }

        public int GetChannelId()
        {
            return 1001;
        }

        public int GetPayType()
        {
            return 1;
        }

        public void Login(string msg, Action<int, string> onResult, Action<int, string> onError)
        {
            throw new NotImplementedException();
        }

        public void Pay(string msg, Action<int, string> onResult, Action<int, string> onError)
        {
#if ENABLE_REMOTE_DEBUG_SDK
            Debug.Log("#WebSocket call pay" );
            Dictionary<string,string> dic = new Dictionary<string, string>();
            dic.Add("action","pay");
            dic.Add("msg",msg);
            dic.Add("requestId",mRequestID.ToString());
            mRequestDatas.Add(mRequestID,new RequestData{onError = onError,onResult = onResult,requestId = mRequestID});
            mRequestID++;
            
            if(webSocket == null)
            {
                Debug.LogError("webSocket is null");
                return;
            }
            
            if(!webSocket.IsOpen)
            {
                Debug.LogError("webSocket is not open");
                return;
            }
            webSocket.Send(JsonConvert.SerializeObject(dic));
#endif
        }

        public string CallApiReturnString(string api)
        {
            return string.Empty;
        }

        public int CallApiReturnInt(string api)
        {
            return 0;
        }

        public bool CallApiReturnBool(string api)
        {
            return false;
        }

        public void CallApiReturnStringWithSafe(string api, Action<string> onResult)
        {
            onResult?.Invoke(string.Empty);
        }

        public void CallApiReturnIntWithSafe(string api, Action<int> onResult)
        {
            onResult?.Invoke(0);
        }

        public void CallApiReturnBoolWithSafe(string api, Action<bool> onResult)
        {
            onResult?.Invoke(false);
        }

        public void RequestAsyncFuntion(string api, string msg, Action<int, string> onResult, Action<int, string> onError)
        {
#if ENABLE_REMOTE_DEBUG_SDK
            
            Dictionary<string,string> dic = new Dictionary<string, string>();
            dic.Add("action",api);
            dic.Add("msg",msg);
            dic.Add("requestId",mRequestID.ToString());
            mRequestDatas.Add(mRequestID,new RequestData{onError = onError,onResult = onResult,requestId = mRequestID});
            mRequestID++;
            
            if(webSocket == null)
            {
               Debug.LogError("webSocket is null");
               return;
            }
            
            if(!webSocket.IsOpen)
            {
                Debug.LogError("webSocket is not open");
                return;
            }
            Debug.Log("#WebSocket call:" + JsonConvert.SerializeObject(dic) );
            webSocket.Send(JsonConvert.SerializeObject(dic));
#endif
        }

        public void Log(string msg)
        {
            Debug.Log(msg);
        }

        public void Restart()
        {
        }
        public void Destroy()
        {
#if ENABLE_REMOTE_DEBUG_SDK
            if (webSocket != null)
            {
                webSocket.Close();
                webSocket = null;
            }
#endif
        }
    }
}