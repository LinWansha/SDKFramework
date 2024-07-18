using System;

namespace Sdkhubv2.Runtime.Platform
{
    public interface IHabbySDKAPI
    {
        void OnCreate();
        #region channel
        int GetChannelId();
        int GetPayType();
        void Login(string msg,Action<int,string> onResult,Action<int,string> onError);
        void Pay(string msg,Action<int,string> onResult,Action<int,string> onError);
        #endregion
        
        /// <summary>
        /// 调用返回string的api,同步调用,可能不太主线程
        /// </summary>
        /// <param name="api">api名字</param>
        /// <returns></returns>
        string CallApiReturnString(string api);
        /// <summary>
        /// 调用返回int的api,同步调用,可能不太主线程
        /// </summary>
        /// <param name="api">api名字</param>
        /// <returns></returns>
        int CallApiReturnInt(string api);
        
        /// <summary>
        /// 调用返回bool的api,同步调用,可能不太主线程
        /// </summary>
        /// <param name="api">api名字</param>
        /// <returns></returns>
        bool CallApiReturnBool(string api);
        
        void CallApiReturnStringWithSafe(string api,Action<string> onResult);
        void CallApiReturnIntWithSafe(string api,Action<int> onResult);
        void CallApiReturnBoolWithSafe(string api,Action<bool> onResult);
        
        void RequestAsyncFuntion(string api,string msg,Action<int,string> onResult,Action<int,string> onError);
        void Log(string msg);
        void Restart();
        void Destroy();
    }
}