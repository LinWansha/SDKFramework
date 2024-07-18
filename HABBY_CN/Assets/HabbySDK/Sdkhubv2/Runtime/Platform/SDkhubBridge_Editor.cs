using System;
using HabbySDK.HabbyTimerManager;

namespace Sdkhubv2.Runtime.Platform
{
    public class SDkhubBridge_Editor:IHabbySDKAPI
    {
        public void OnCreate()
        {
        }

        public int GetChannelId()
        {
            return 0;
        }

        public int GetPayType()
        {
            return 0;
        }

        public void Login(string msg, Action<int, string> onResult, Action<int, string> onError)
        {
            throw new NotImplementedException();
        }

        public void Pay(string msg, Action<int, string> onResult, Action<int, string> onError)
        {
            throw new NotImplementedException();
        }

        public string CallApiReturnString(string api)
        {
            throw new NotImplementedException();
        }

        public int CallApiReturnInt(string api)
        {
            throw new NotImplementedException();
        }

        public bool CallApiReturnBool(string api)
        {
            throw new NotImplementedException();
        }

        public void CallApiReturnStringWithSafe(string api, Action<string> onResult)
        {
            MainThreadTaskQueue.EnqueueTask(()=>
            {
                onResult?.Invoke(string.Empty);
            });
        }

        public void CallApiReturnIntWithSafe(string api, Action<int> onResult)
        {
            MainThreadTaskQueue.EnqueueTask(()=>
            {
                onResult?.Invoke(0);
            });
        }

        public void CallApiReturnBoolWithSafe(string api, Action<bool> onResult)
        {
            MainThreadTaskQueue.EnqueueTask(()=>
            {
                onResult?.Invoke(false);
            });
        }

        public void RequestAsyncFuntion(string api,string msg, Action<int, string> onResult, Action<int, string> onError)
        {
            throw new NotImplementedException();
        }

        public void Log(string msg)
        {
            throw new NotImplementedException();
        }

        public void Restart()
        {
            throw new NotImplementedException();
        }

        public void Destroy()
        {
            throw new NotImplementedException();
        }
    }
}