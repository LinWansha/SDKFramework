using System;
using Habby;
using Habby.Account;
using Habby.Events;
using HabbySDK.HabbyTimerManager;
using UnityEngine;

namespace Sdkhubv2.Runtime.Platform
{
    
  
#if UNITY_ANDROID
    public class SDkhubBridge_Android :IHabbySDKAPI
    {

        
      
        private readonly string ANDROID_SDK_API_CLASS = "com.habby.sdkhubv2.HabbySDKAPI";
        private  AndroidJavaClass apiClass ;
        
        private void HandleJavaCallback(string eventName, string msg)
        {
            Debug.LogWarning("--- ### unity handleJavaCallback2 eventName=" + eventName + " msg=" + msg);
            HabbyEventManagerV1.Instance.DispatchWithSafe(eventName, msg);
        }

        #region channel api
        public int GetChannelId()
        {
            if(apiClass!=null)
                return apiClass.CallStatic<int>("getChannelId");
            return -1;
        }

        public int GetPayType()
        {
            if(apiClass!=null)
                return apiClass.CallStatic<int>("getPayType");
            return -1;
        }

        public void Login(string msg,Action<int,string> onResult,Action<int,string> onError)
        {
            throw new System.NotImplementedException();
        }

        public void Pay(string msg, Action<int, string> onResult, Action<int, string> onError)
        {
            if(apiClass!=null)
                 apiClass.CallStatic("pay",msg,new AndroidNativeHandler(onResult,onError));
        }

        public void Logout(Action<int,string> onResult,Action<int,string> onError)
        {
            throw new System.NotImplementedException();
        }
        
        public void pay(string msg,Action<int,string> onResult,Action<int,string> onError)
        {
            throw new System.NotImplementedException();
        }
        
        public void reportGameEvent(string eventName,string msg)
        {
            throw new System.NotImplementedException();
        }
        public void initAds(string msg,Action<int,string> onResult,Action<int,string> onError)
        {
            throw new System.NotImplementedException();
        }
        
        public void loadAd(string adId,Action<int,string> onResult,Action<int,string> onError)
        {
            throw new System.NotImplementedException();
        }
        
        public void showAd(string adId,Action<int,string> onResult,Action<int,string> onError)
        {
            throw new System.NotImplementedException();
        }
        
        public void openMarket(string msg)
        {
            throw new System.NotImplementedException();
        }
        
        #endregion
        public void OnCreate()
        {
            apiClass = new AndroidJavaClass(ANDROID_SDK_API_CLASS);
            if(apiClass!=null)
                 apiClass.CallStatic("setEventHandler",new SDKHubAndroidEventListener(HandleJavaCallback));
        }

        #region sync api
        public string CallApiReturnString(string api)
        {
            if(apiClass!=null)
                return apiClass.CallStatic<string>("callApiReturnString",api);
            return string.Empty;
        }
        public int CallApiReturnInt(string api)
        {
            if(apiClass!=null)
                return apiClass.CallStatic<int>("callApiReturnInt",api);
            return 0;
        }

        public bool CallApiReturnBool(string api)
        {
            if(apiClass!=null)
                return apiClass.CallStatic<bool>("callApiReturnBool",api);
            return false;
        }

        public void CallApiReturnStringWithSafe(string api, Action<string> onResult)
        {
            string result = CallApiReturnString(api);
            MainThreadTaskQueue.EnqueueTask(()=>
            {
                onResult?.Invoke(result);
            });
        }

        public void CallApiReturnIntWithSafe(string api, Action<int> onResult)
        {
            int result = CallApiReturnInt(api);
            MainThreadTaskQueue.EnqueueTask(()=>
            {
                onResult?.Invoke(result);
            });
        }

        public void CallApiReturnBoolWithSafe(string api, Action<bool> onResult)
        {
            bool result = CallApiReturnBool(api);
            MainThreadTaskQueue.EnqueueTask(()=>
            {
                onResult?.Invoke(result);
            });
        }

        #endregion
        

        public void RequestAsyncFuntion(string api,string msg,Action<int,string> onResult,Action<int,string> onError)
        {
            if (apiClass != null)
            {
                apiClass.CallStatic("requestAsyncFuntion",api,msg,new AndroidNativeHandler(onResult,onError));
            }
                 
        }

        public void Log(string msg)
        {
            if (apiClass != null)
            {
                apiClass.CallStatic("nativeLogD",msg);
            }
        }

        public void Restart()
        {
            throw new NotImplementedException();
        }

        public void Destroy()
        {
            throw new NotImplementedException();
        }
        

        #region listener

        protected class SDKHubAndroidEventListener: AndroidJavaProxy
        {
            private const string ANDROID_SDK_LISTENER = "com.habby.sdkhubv2.interfaces.INotifyGameEngine";

            private Action<string, string> mHandler;
            // private Dictionary<string, string> mEventMap;
            public SDKHubAndroidEventListener(Action<string, string> handler) : base(ANDROID_SDK_LISTENER)
            {
                mHandler = handler;
            }
            public void notify(String eventName, String msg)
            {
                SDKHubLog.LogWarning("--- ### unity handleAndroidEvent eventName=" + eventName + " msg=" + msg);
                MainThreadTaskQueue.EnqueueTask(()=>mHandler?.Invoke(eventName, msg));
            }
        }
        #endregion
    }
#endif 
}