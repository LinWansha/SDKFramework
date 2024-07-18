using System;
using HabbySDK.Sdkhubv2.Runtime.tools;

namespace Sdkhubv2.Runtime.tools
{
    public class WeChatAPIUtil
    {
        public static bool IsInstalled
        {
            get
            {
#if ENABLE_REMOTE_DEBUG_SDK
            return true;
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
                
                return PlatformUtil.callStaticBool("isWeChatInstalled");
#endif
                return true;
            }
        }
        
        public static void RequestShare( Action<int, string> onResult, Action<int, string> onError)
        {
            //TODO: Implement this method
            onResult?.Invoke(0,"");
        }
        

        public static void RequestLoginAuthToken( Action<int, string> onResult, Action<int, string> onError)
        {
            try
            {
                HabbySDKHubManager.Instance.PlatformBridge.RequestAsyncFuntion(HabbyApiConst.API_WEICHAT_OAUTH_GET_TOKEN,"", (code, msg) =>
                {
                    onResult?.Invoke(code,msg);
                }, (code, msg) =>
                {
                    onError?.Invoke(code,msg);
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