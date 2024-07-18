using System;
using Habby;
using Habby.Base;
using Habby.SDK.Tool;
using HabbySDK.HabbyTimerManager;
using Sdkhubv2.Runtime.Platform;
using Sdkhubv2.Runtime.Platform.Channel;
using Sdkhubv2.Runtime.tools;
using UnityEngine;

namespace Sdkhubv2.Runtime
{
    public class HabbySDKHubManager: MonoBehaviour
    {
        private IHabbySDKAPI mBridge;
        protected static HabbySDKHubManager _instance;
        public SDKHubTimer timer;
        private bool mIsInit = false;
        public static HabbySDKHubManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType(typeof(HabbySDKHubManager)) as HabbySDKHubManager;
                    if (_instance == null)
                    {
                        _instance = new GameObject("HabbySDKHubManager").AddComponent<HabbySDKHubManager>();
                        // ios sendMessage need a Unity object
                        _instance.Init();
                        DontDestroyOnLoad(_instance.gameObject);

                    }
                }

                return _instance;
            }
        }
        
        public IHabbySDKAPI PlatformBridge
        {
            get
            {
                return mBridge;
            }
        }
        
        protected IHabbyChannel mChannel;
        public IChannelProvider ChannelProvider
        {
            set;
            get;
        }
        public IHabbyChannel Channel
        {
            get
            {
                if(ChannelProvider == null)
                    throw new Exception("ChannelProvider is null");
                if (null == mChannel)
                {
                    mChannel = ChannelProvider?.GetChannel();
                }
                return mChannel;
            }
        }

        public void Init()
        {
            if(mIsInit)
                return;
            
            mIsInit = true;
            
            timer = new SDKHubTimer();
             
            if (null == mBridge)
            {
#if UNITY_EDITOR && !ENABLE_REMOTE_DEBUG_SDK
            mBridge = new SDkhubBridge_Editor();
#elif UNITY_EDITOR && ENABLE_REMOTE_DEBUG_SDK
                mBridge = new SDkhubBridge_Editor_Remote_Debug();
#elif UNITY_ANDROID
           mBridge = new SDkhubBridge_Android();
#elif UNITY_IPHONE
            throw new Exception("not implement");
#endif
                mBridge.OnCreate();
                   
            }
        }
        
        #region update

        private event Action<float> OnUpdateEvent = (t) => { };
        public event Action<float> OnUpdate {
            add
            {
                //Log.Debug("Register main loop update from " + value.Method.DeclaringType.Name);
                OnUpdateEvent += value;
            }
            remove
            {
                //Log.Debug("Unregister main loop update from " + value.Method.DeclaringType.Name);
                OnUpdateEvent -= value;
            }
        }
         
        void Update()
        {
            OnUpdateEvent.Invoke(Time.deltaTime);
            MainThreadTaskQueue.ExecuteTasks();
        }
   
        #endregion

        
    }
}