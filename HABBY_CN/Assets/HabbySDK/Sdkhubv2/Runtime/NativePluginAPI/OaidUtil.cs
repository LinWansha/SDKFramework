using System;
using HabbySDK.Sdkhubv2.Runtime.tools;

namespace Sdkhubv2.Runtime.tools
{
    public class OaidUtil
    {
        
        public static void IsSupportOaid(Action<bool> onResult)
        {
            #if ENABLE_REMOTE_DEBUG_SDK
            onResult?.Invoke(true);
            #endif
#if UNITY_ANDROID
            HabbySDKHubManager.Instance.PlatformBridge.CallApiReturnBoolWithSafe(HabbyApiConst.API_GET_IS_OAID_SUPPORT,onResult);
#else
            onResult?.Invoke(false);
#endif
        }
        
        public static void IsOaidReady(Action<bool> onResult)
        {
#if ENABLE_REMOTE_DEBUG_SDK
            onResult?.Invoke(true);
#endif
#if UNITY_ANDROID
            HabbySDKHubManager.Instance.PlatformBridge.CallApiReturnBoolWithSafe(HabbyApiConst.API_GET_IS_OAID_READY,onResult);
#else
            onResult?.Invoke(false);
#endif
        }
        
        public static void GetOaid(Action<string> onResult)
        {
#if ENABLE_REMOTE_DEBUG_SDK
            onResult?.Invoke("fake oaid");
#endif
#if UNITY_ANDROID
             HabbySDKHubManager.Instance.PlatformBridge.CallApiReturnStringWithSafe(HabbyApiConst.API_GET_OAID,onResult);
#else
            onResult?.Invoke(string.Empty);
#endif
        }
        
        public static string Oaid
        {
            get
            {
#if ENABLE_REMOTE_DEBUG_SDK
            return "fake oaid";
#endif
#if UNITY_ANDROID
                
                return PlatformUtil.callStaticString("getOaid");
#endif
                return string.Empty;
            }
        }
        
    }
}