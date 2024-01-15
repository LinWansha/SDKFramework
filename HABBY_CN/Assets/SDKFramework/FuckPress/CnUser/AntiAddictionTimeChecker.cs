using System;
using UnityEngine;

namespace Habby.CNUser
{
    public class AntiAddictionTimeChecker
    {
        public int VisitorOnlineTimeLimit = 0; // 游玩模式可以在线时长 CN不允许游玩模式
        public int Day_OnlineTimeLimit = 0; // 工作日可以在线时长   CN工作日不可以玩
        public int Holiday_OnlineTimeLimit = 60 * 60; // 节假日可以游玩时间   GM

        public int NotifyWhenRemainTimeLeft = 10 * 60; // 最后剩余十分钟的时候的提醒  GM

        //上次登录时间
        private float _last_time = 0;
        private bool _remain_time_notified;
        private bool _ticking;
        private bool _data_dirty;
        private bool _is_holiday;

        private readonly JJR _holiday_checker = new JJR();

        private int _interval = 20;

        /// <summary>
        /// 进入游戏
        /// </summary>
        public void StartTimeCounter()
        {
            _last_time = Time.realtimeSinceStartup;
            _ticking = true;
            _is_holiday = _holiday_checker.IsHoliday(TimerHelper.GetNowTime());
#if CHANNEL_CN
            if (HabbyCloudConfigManager.Instance.CloudData != null)
            {
                _interval = HabbyCloudConfigManager.Instance.CloudData.heartbeatInterval;
            }
            _interval = Math.Max(_interval,30);
#else
            _interval = 100;
#endif
        }

        /// <summary>
        /// 离开游戏
        /// </summary>
        public void StopTimeCounter(UserAccount account)
        {
            if (!_ticking) return;
            _ticking = false;

            float curr_time = Time.realtimeSinceStartup;
            float online_time = curr_time - _last_time;

            int seconds = (int)(online_time);
            if (seconds < 1) return;
            if (account.AgeRange != UserAccount.AgeLevel.Adult) account.AddOnline(seconds);
            _data_dirty = true;
            //刷新_last_time
            _last_time = curr_time;

            if (account != null && account.Online != null)
            {
            }
        }

        public void ResetNotice()
        {
            _remain_time_notified = false;
        }

        public TimeRegulation CheckOnlineTime(UserAccount account)
        {
            if (!_ticking) return TimeRegulation.None;
            if (account == null) return TimeRegulation.Exit;
            HLog.LogFormat("AntiAddictionTimeChecker CheckOnlineTime age={0} time={1}", account.AgeRange,
                Time.realtimeSinceStartup);
            // 用户没有登陆
            if (_last_time == 0 || account.Online == null) return TimeRegulation.None;

            //累计在线时间限制
            float curr_time = Time.realtimeSinceStartup;
            float online_time = curr_time - _last_time;

            int seconds = (int)(online_time);

            if (seconds >= _interval)
            {
                account.AddOnline(seconds);
                _data_dirty = true;
                _last_time = curr_time;
                UploadData(account.Online);
                seconds = 0;
            }

            HLog.LogFormat("AntiAddictionTimeChecker today={0},total={1}", account.Online.Today, account.Online.Total);
            int remain;
            switch (account.AgeRange)
            {
                case UserAccount.AgeLevel.Unknown:
                    //15天累计在线不超过60min
                    // 新版防沉迷游客不能进入 
                    return TimeRegulation.Exit;
                    // remain = VisitorOnlineTimeLimit - account.Online.Total - seconds;
                    // if (remain <= 0)
                    // {
                    //     return TimeRegulation.Exit;
                    // }
                    // else if (remain <= NotifyWhenRemainTimeLeft && !_remain_time_notified)
                    // {
                    //     _remain_time_notified = true;
                    //     return TimeRegulation.RemainTenMinute;
                    // }
                    break;
                case UserAccount.AgeLevel.Under8:
                case UserAccount.AgeLevel.Under16:
                case UserAccount.AgeLevel.Under18:
                    //登录时段限制
                    if (isRestrictTime()) return TimeRegulation.Exit; //ForbidLogin

                    //累计在线时间不超过**
                    remain = gamingTimeLeft(account) - seconds;
                    if (remain <= 0)
                    {
                        return TimeRegulation.Exit;
                    }
                    else if (remain <= NotifyWhenRemainTimeLeft && !_remain_time_notified)
                    {
                        _remain_time_notified = true;
                        return TimeRegulation.RemainTenMinute;
                    }
                    else if (isLastTenTime() && !_remain_time_notified)
                    {
                        _remain_time_notified = true;
                        return TimeRegulation.RemainTenMinute;
                    }

                    break;
            }

            return TimeRegulation.None;
        }

        public bool IsHolidy
        {
            get { return _is_holiday; }
        }

        public bool IsBadTime(UserAccount account)
        {
            switch (account.AgeRange)
            {
                case UserAccount.AgeLevel.Under8:
                case UserAccount.AgeLevel.Under16:
                case UserAccount.AgeLevel.Under18:
                    //登录时段限制
                    return isRestrictTime();
            }

            return false;
        }

        public bool HasTimeLeft(UserAccount account)
        {
            Debug.Log("##### HasTimeLeft _last_time : " + _last_time);
            //尝试在登录判定时清除久的时间数据
            _last_time = 0;
            if (account == null || account.Online == null) return true;
            switch (account.AgeRange)
            {
                case UserAccount.AgeLevel.Unknown:
                    return VisitorOnlineTimeLimit > account.Online.Total;
                case UserAccount.AgeLevel.Under8:
                case UserAccount.AgeLevel.Under16:
                case UserAccount.AgeLevel.Under18:
                    //登录时段限制
                    return gamingTimeLeft(account) > 70;
            }

            return true;
        }

        private bool isRestrictTime()
        {
            //// 旧版本登陆时间限制  
            //DateTime now = TimerHelper.GetNowTime();
            //int hour = now.Hour;
            //int minute = now.Minute;
            //return (hour < 8 || (hour == 8 && minute == 0) || hour >= 22);
            //---------------------------------- GM
            _is_holiday = _holiday_checker.IsHoliday(TimerHelper.GetNowTime());
            if (!_is_holiday)
                return true;

            DateTime now = TimerHelper.GetNowTime();
            int hour = now.Hour;
            int minute = now.Minute;
            return (hour < 20 || hour >= 21);
        }

        private bool isLastTenTime()
        {
            DateTime now = TimerHelper.GetNowTime();
            int hour = now.Hour;
            int minute = now.Minute;
            return hour == 20 && minute == 50;
        }


        private int gamingTimeLeft(UserAccount account)
        {
            //避免登录时_is_holiday尚未进行判定
            _is_holiday = _holiday_checker.IsHoliday(TimerHelper.GetNowTime());

            int onlineTimeLimit;
            if (_is_holiday)
            {
                onlineTimeLimit = Holiday_OnlineTimeLimit;
            }
            else
            {
                onlineTimeLimit = Day_OnlineTimeLimit;
            }

            return (onlineTimeLimit - account.Online.Today);
        }

        public void UploadData(UserOnlineData data)
        {
            if (data == null || !_data_dirty) return;
            _data_dirty = false;

            if (data.segments.Count < 2) return;

            // HabbyUserClient.Instance.UpdateUserOnlineData(AccountManager.Instance.CurrentAccount, data.segments,
            //     (response) =>
            //     {
            //         if (response.code == SyncOnlineDataResponse.CODE_SUCCESS)
            //         {
            //             if (data != null) data.segments.Clear();
            //         }
            //     }, (error) =>
            //     {
            //     });
        }

        public enum TimeRegulation
        {
            None = 0,
            ForbidLogin,
            RemainTenMinute,
            Exit
        }
    }
}