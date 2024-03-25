using System;
using System.Collections.Generic;
using SDKFramework.Account.Net;
using SDKFramework.Utils;

namespace SDKFramework.Account.DataSrc
{
    [Serializable]
    public class UserOnlineData
    {
        private const string KEY_ONLINE_MINUTES_TODAY = "online_today_{0}";
        private const string KEY_ONLINE_MINUTES_TOTAL = "online_total";

        public UserDataEntry totalOnlineSeconds;
        public UserDataEntry todayOnlineSeconds;

        public List<UserOnlieSegment> segments;
        public UserOnlineData()
        {
            totalOnlineSeconds = createTotal();
            todayOnlineSeconds = createDaily();
            segments = new List<UserOnlieSegment>();
        }

        public void Refresh()
        {
            if (totalOnlineSeconds == null) totalOnlineSeconds = createTotal();

            string key = string.Format(KEY_ONLINE_MINUTES_TODAY, DateTime.Now.DayOfYear);
            if (todayOnlineSeconds == null || !key.Equals(todayOnlineSeconds.name)) todayOnlineSeconds = createDaily(key);

            if (segments == null) segments = new List<UserOnlieSegment>();
        }

        public void Add(int seconds)
        {
            todayOnlineSeconds.value += seconds;
            totalOnlineSeconds.value += seconds;

            // long now = TimerHelper.Get1970ToNowMilliseconds();
            // UserOnlieSegment s = new UserOnlieSegment {
            //     startAt = now - seconds * 1000,
            //     endAt = now
            // };
            UserOnlieSegment s = new UserOnlieSegment {
                startAt = TimerHelper.GetBeforeUTC2String(seconds),
                endAt = TimerHelper.GetNowUTC2String()
            };
            segments.Add(s);
        }

        public int Total { get { return totalOnlineSeconds.value; } set { totalOnlineSeconds.value = value; } }
        public int Today { get { return todayOnlineSeconds.value; } set { todayOnlineSeconds.value = value; } }

        private static UserDataEntry createTotal()
        {
            return new UserDataEntry() {
                name = KEY_ONLINE_MINUTES_TOTAL,
                value = 0
            };
        }

        private static UserDataEntry createDaily(string dailyKey = null)
        {
            if (dailyKey == null) {
                DateTime now = DateTime.Now;
                dailyKey = string.Format(KEY_ONLINE_MINUTES_TODAY, now.DayOfYear);
            }
            return new UserDataEntry() {
                name = dailyKey,
                value = 0
            };
        }
    }
}
