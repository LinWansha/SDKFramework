using System;
using HabbySDK.HabbyTimerManager;
using UnityEngine;

namespace Sdkhubv2.Runtime.Platform
{
#if UNITY_ANDROID
    public class AndroidNativeHandler: AndroidJavaProxy
    {
        

        private Action<int,string> mOnResult;
        private Action<int,string> mOnError;
        private uint mRequestId;

        public uint RequestId => mRequestId;

        public AndroidNativeHandler(Action<int,string> onResult,Action<int,string> onError) : base("com.habby.sdkhubv2.interfaces.IFunctionHandler")
        {
            mRequestId = HabbyRequestIdUtil.GetRequestId();
            mOnResult = onResult;
            mOnError = onError;
        }
        
        public void onResult(int code, string msg)
        {
            Debug.Log("onResult code:"+code+" msg:"+msg);
            MainThreadTaskQueue.EnqueueTask(() =>
            {
                mOnResult?.Invoke(code,msg);
            });
           
        }

        public void onError(int code, string msg)
        {
            Debug.Log("onError code:"+code+" msg:"+msg);
            MainThreadTaskQueue.EnqueueTask(() =>
            {
                mOnError?.Invoke(code,msg);
            });
        }

        public int getRequestId()
        {
            return 0;
        }
        public void Destory()
        {
            mOnResult = null;
            mOnError = null;
        }

    }
#endif
}