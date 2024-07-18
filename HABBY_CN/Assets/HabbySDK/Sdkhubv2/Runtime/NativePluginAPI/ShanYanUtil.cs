using System;
using Habby.Events;

namespace Sdkhubv2.Runtime.tools
{
    public class ShanYanUtil
    {
        /// <summary>
        /// 闪验是否已经初始化成功（是否可以调用闪验RequestShanYanAuthToken接口）
        /// </summary>
        /// <returns></returns>
        public static bool IsShanYanValid()
        {
#if ENABLE_REMOTE_DEBUG_SDK
            return false;
#else
            return HabbySDKHubManager.Instance.PlatformBridge.CallApiReturnBool(HabbyApiConst.API_IS_READY);
#endif

        }
        /// <summary>
        /// 是否有sim卡
        /// </summary>
        /// <returns></returns>
        public static bool hasSimeCard()
        {
#if ENABLE_REMOTE_DEBUG_SDK
            return true;
#else
            return HabbySDKHubManager.Instance.PlatformBridge.CallApiReturnBool(HabbyApiConst.API_HAS_SIMCARD);
#endif
            
        }
        /// <summary>
        /// 获取闪验初始化状态
        /// </summary>
        /// <returns></returns>
        public static string getState()
        {
            return HabbySDKHubManager.Instance.PlatformBridge.CallApiReturnString(HabbyApiConst.API_GET_STATE);
        }
        /// <summary>
        /// 获取闪验token
        /// </summary>
        /// <param name="onResult"></param>
        /// <param name="onError"></param>
        public static void RequestShanYanAuthToken(Action<int, string> onResult, Action<int, string> onError)
        {
            try
            {
                HabbySDKHubManager.Instance.PlatformBridge.RequestAsyncFuntion(HabbyApiConst.API_GET_SHANYAN_TOKEN,"", (code, msg) =>
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