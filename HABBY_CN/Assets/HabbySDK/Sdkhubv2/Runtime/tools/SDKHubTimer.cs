
using Habby.Account;
using Sdkhubv2.Runtime;

namespace Habby.SDK.Tool
{
 
    using System;
    using System.Collections.Generic;

    public class SDKHubTimer
    {
        private static int mTimerId = 0;

        private static Dictionary<int, SDKHubTimer> mTimersDict = new Dictionary<int, SDKHubTimer>(32);
        private static Dictionary<int, SDKHubTimer> mTempDict = new Dictionary<int, SDKHubTimer>(32);
        private static List<int> mTempList = new List<int>();
        private static Stack<SDKHubTimer> mReusable = new Stack<SDKHubTimer>(32);
        public static int MAXIMUM_REUSABLE_COUNT = 8;


        static SDKHubTimer()
        {
            HabbySDKHubManager.Instance.OnUpdate += Loop;
        }


        private static void Loop(float aDeltaTime)
        {
            foreach (var pair in mTempDict)
            {
                if (pair.Value.Active && pair.Key == pair.Value.mId)
                {
                    mTimersDict.Add(pair.Key, pair.Value);
                }
            }
            mTempDict.Clear();
            foreach (var pair in mTimersDict)
            {
                if (pair.Value.Active && pair.Key == pair.Value.mId)
                {
                    pair.Value.Step(aDeltaTime);
                }
                else
                {
                    mTempList.Add(pair.Key);
                }
            }
            foreach (var key in mTempList)
            {
                mTimersDict.Remove(key);
            }
            mTempList.Clear();
            if (mReusable.Count > MAXIMUM_REUSABLE_COUNT * 1.5f)
            {
                Truncate();
            }
        }


        internal bool Active { get; private set; }
        private Action<int> OnTrigger;
        private int mId;
        private float mInterval;
        private int mTotlaCount;
        private int mCount;
        private float mTime;
        private float mTimeScale = 1;


        private static SDKHubTimer Get(int aTimerId)
        {
            SDKHubTimer sdkHubTimer = null;
            if (mReusable.Count > 0)
            {
                sdkHubTimer = mReusable.Pop();
            }
            sdkHubTimer = new SDKHubTimer();
            mTempDict.Add(aTimerId, sdkHubTimer);
            return sdkHubTimer;
        }


        public static int Execute(Action<int> TimerAction, float aInterval, int aCount = 1,
            bool aExecuteNow = false, float aTimeScale = 1)
        {
            if (TimerAction == null)
            {
                SDKHubLog.LogWarning("executing null action with SDKHubTimer!");
                return -1;
            }
            mTimerId++;
            SDKHubTimer sdkHubTimer = Get(mTimerId);
            sdkHubTimer.Active = true;
            sdkHubTimer.OnTrigger = TimerAction;
            sdkHubTimer.mId = mTimerId;
            sdkHubTimer.mInterval = aInterval;
            sdkHubTimer.mTotlaCount = aCount;
            sdkHubTimer.mCount = 0;
            sdkHubTimer.mTime = 0;
            sdkHubTimer.mTimeScale = aTimeScale;

            if (aExecuteNow)
            {
                sdkHubTimer.Trigger(sdkHubTimer.mCount);
                sdkHubTimer.mCount++;
            }

            return mTimerId;
        }


        public static void Cancel(int aTimerId)
        {
            SDKHubTimer sdkHubTimer = null;
            if (!mTimersDict.TryGetValue(aTimerId, out sdkHubTimer))
            {
                mTempDict.TryGetValue(aTimerId, out sdkHubTimer);
            }
            if (null != sdkHubTimer && sdkHubTimer.mId == aTimerId)
            {
                sdkHubTimer.Cancel();
            }
        }

        public static void SetTimeScale(int aTimerId, float aTimeScale)
        {
            SDKHubTimer sdkHubTimer = null;
            if (!mTimersDict.TryGetValue(aTimerId, out sdkHubTimer))
            {
                mTempDict.TryGetValue(aTimerId, out sdkHubTimer);
            }
            if (null != sdkHubTimer && sdkHubTimer.mId == aTimerId)
            {
                sdkHubTimer.SetTimeScale(aTimeScale);
            }
        }

        //check if current interval should be triggered
        private bool IntervalStep(float aDeltaTime)
        {
            if (mInterval > mTime)
            {
                return false;
            }
            mTime = 0;
            if (mTotlaCount >= ++mCount)
            {
                return true;
            }
            return false;
        }


        internal void Step(float aDeltaTime)
        {
            if (IntervalStep(aDeltaTime))
            {
                Trigger(mCount);
            }
            mTime += aDeltaTime * mTimeScale;
        }


        internal void Trigger(int count)
        {
            //protected call
            try
            {
                OnTrigger(count);
            }
            catch (Exception e)
            {
                //log the exception & advance this timer to its end phase
                SDKHubLog.LogWarning($"exception while executing timer:\n{e.Message} \n{e.StackTrace}");
            }
            finally
            {
                if (mCount >= mTotlaCount)
                {
                    Cancel();
                }
            }
        }


        internal void Cancel()
        {
            if (Active)
            {
                Active = false;
                OnTrigger = null;

                if (mReusable.Contains(this))
                {
                    SDKHubLog.LogWarning("Repeated add to resuable!");
                }
                else
                {
                    mReusable.Push(this);
                }
            }
        }

        internal void SetTimeScale(float aTimeScale)
        {
            if (Active)
            {
                mTimeScale = aTimeScale;
            }
        }

        //truncate inactive timers
        internal static void Truncate()
        {
            int count = Math.Max(MAXIMUM_REUSABLE_COUNT, mReusable.Count / 10);
            while (mReusable.Count > count)
            {
                mReusable.Pop();
            }
        }


        //remove all but active external timer
        internal static void Clear()
        {
            foreach (var pair in mTimersDict)
            {
                if (pair.Key == pair.Value.mId)
                {
                    pair.Value.Cancel();
                }
            }
            foreach (var pair in mTempDict)
            {
                if (pair.Key == pair.Value.mId)
                {
                    pair.Value.Cancel();
                }
            }
            mTimersDict.Clear();
            mTempDict.Clear();
            Truncate();
        }
    }

}