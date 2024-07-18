using System;
using Habby.Events;
using HabbySDK.Sdkhubv2.Runtime.tools;
using UnityEngine;

namespace Sdkhubv2.Runtime.tools
{
    public class QQAPIUtil
    {
        public static void IsInstall(Action<bool> onResult)
        {
#if ENABLE_REMOTE_DEBUG_SDK
            onResult?.Invoke(true);
#endif
            HabbySDKHubManager.Instance.PlatformBridge.CallApiReturnBoolWithSafe(HabbyApiConst.API_GET_QQ_INSTALLED,onResult);
        }
        public static bool IsInstalled
        {
            get
            {
#if ENABLE_REMOTE_DEBUG_SDK
            return true;
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
                return PlatformUtil.callStaticBool("isQQInstalled");
#endif
                return true;
            }
        }
        public static void AddQQGroup(string qqGroupKey)
        {
            HabbySDKHubManager.Instance.PlatformBridge.RequestAsyncFuntion(HabbyApiConst.API_ADD_QQ_GROUP,qqGroupKey,null,null);
        }
        
        public static void IsSupportToQZone(Action<bool> onResult)
        {
            HabbySDKHubManager.Instance.PlatformBridge.CallApiReturnBoolWithSafe(HabbyApiConst.API_GET_QQ_SUPPORT_SHARE_ZONE,onResult);
        }
        
        public static void IsSupportShareToQQ(Action<bool> onResult)
        {
            HabbySDKHubManager.Instance.PlatformBridge.CallApiReturnBoolWithSafe(HabbyApiConst.API_GET_QQ_SUPPORT_SHARE_QQ,onResult);
        }
        public static void RequestQqLoginAuthToken(Action<int, string> onResult, Action<int, string> onError)
        {
            try
            {
                HabbySDKHubManager.Instance.PlatformBridge.RequestAsyncFuntion(HabbyApiConst.API_GET_QQ_LOGIN_TOKEN,"", (code, msg) =>
                {
                    onResult?.Invoke(code,msg);
                    HabbyEventManagerV1.Instance.Dispatch(HabbyGameEvent.EVENT_QQ_LOGIN_RESULT, code ,msg);
                }, (code, msg) =>
                {
                    onError?.Invoke(code,msg);
                    HabbyEventManagerV1.Instance.Dispatch(HabbyGameEvent.EVENT_QQ_LOGIN_RESULT, false ,msg);
                });
            }
            catch (Exception e)
            {
               onError?.Invoke(-1,e.StackTrace);
               #if UNITY_EDITOR
               throw;
               #endif
            }
          
        }
        public static void RequestQqLogout(Action<int, string> onResult, Action<int, string> onError)
        {
            try
            {
                HabbySDKHubManager.Instance.PlatformBridge.RequestAsyncFuntion(HabbyApiConst.API_QQ_LOGOUT,"", (code, msg) =>
                {
                    onResult?.Invoke(code,msg);
                    HabbyEventManagerV1.Instance.Dispatch(HabbyGameEvent.EVENT_QQ_LOGIN_RESULT, code ,msg);
                }, (code, msg) =>
                {
                    onError?.Invoke(code,msg);
                    HabbyEventManagerV1.Instance.Dispatch(HabbyGameEvent.EVENT_QQ_LOGIN_RESULT, false ,msg);
                });
            }
            catch (Exception e)
            {
                onError?.Invoke(-1,e.StackTrace);
#if UNITY_EDITOR
                throw;
#endif
            }
          
        }
        
        
        
        
        
    }
}