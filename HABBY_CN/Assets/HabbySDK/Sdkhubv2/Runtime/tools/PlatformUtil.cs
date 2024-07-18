using Sdkhubv2.Runtime;
using Sdkhubv2.Runtime.Platform.Channel;
using UnityEngine;

namespace HabbySDK.Sdkhubv2.Runtime.tools
{
    public class PlatformUtil
    {
        public static bool isEditor()
        {
           #if UNITY_EDITOR 
                return true;
                #else
                return false;
           #endif
        }
        
        public static bool isAndroid()
        {
#if UNITY_ANDROID 
            return true;
#else
                return false;
#endif
        }
        
        public static bool isIOS()
        {
#if UNITY_IPHONE 
            return true;
#else
                return false;
#endif
        }
        
        
        
        public static bool isOfficial()
        {
           return HabbySDKHubManager.Instance.Channel.GetChannelId() == (int)EHabbyChannel.Official;
        }
        
        #if UNITY_ANDROID
        private static readonly string ANDROID_SDK_API_CLASS = "com.habby.sdkhubv2.HabbySDKAPI";
        private static AndroidJavaClass apiClass ;
        #endif
        public static bool callStaticBool(string methodName)
        {
           
#if UNITY_ANDROID
                if (apiClass == null)
                {
                    apiClass = new AndroidJavaClass(ANDROID_SDK_API_CLASS);
                }
                return apiClass.CallStatic<bool>(methodName);
#endif
                return true;
        }
        
        public static string callStaticString(string methodName)
        {
           
#if UNITY_ANDROID
            if (apiClass == null)
            {
                apiClass = new AndroidJavaClass(ANDROID_SDK_API_CLASS);
            }
            return apiClass.CallStatic<string>(methodName);
#endif
            return string.Empty;
        }
        
        public static int callStaticInt(string methodName)
        {
           
#if UNITY_ANDROID
            if (apiClass == null)
            {
                apiClass = new AndroidJavaClass(ANDROID_SDK_API_CLASS);
            }
            return apiClass.CallStatic<int>(methodName);
#endif
            return 0;
        }
        
        
        public static bool IsSupport(string api)
        {
           
#if UNITY_ANDROID
            if (apiClass == null)
            {
                apiClass = new AndroidJavaClass(ANDROID_SDK_API_CLASS);
            }
            return apiClass.CallStatic<bool>("isSupport",api);
#endif
            return false;
        }
        
        
    }
}