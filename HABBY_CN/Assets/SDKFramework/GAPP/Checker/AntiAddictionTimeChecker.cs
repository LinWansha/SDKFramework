using System;
using UnityEngine;

namespace Habby.CNUser
{
    public class AntiAddictionTimeChecker
    {
        public int Day_OnlineTimeLimit = 0; // 工作日可以在线时长   CN工作日不可以玩
        public int Holiday_OnlineTimeLimit = 60 * 60; // 节假日可以游玩时间   GM


        //上次登录时间
        private float _last_time = 0;
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

            _interval = 100;
        }

        /// <summary>
        /// 离开游戏
        /// </summary>
        public void StopTimeCounter(UserAccount account)
        {
            HLogger.LogFormat("AntiAddictionTimeChecker StopTimeCounter ticking={0}", _ticking);
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
                HLogger.LogFormat("AntiAddictionTimeChecker today={0},total={1}", account.Online.Total,
                    account.Online.Today);
            }
        }

        public TimeRegulation CheckOnlineTime(UserAccount account)
        {
            if (!_ticking) return TimeRegulation.None;
            if (account == null) return TimeRegulation.Exit;
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

            HLogger.LogFormat("AntiAddictionTimeChecker today={0},total={1}", account.Online.Today,
                account.Online.Total);
            int remain;
            if (account.AgeRange != UserAccount.AgeLevel.Adult)
            {
                //登录时段限制
                if (isRestrictTime()) return TimeRegulation.Exit; //ForbidLogin

                //累计在线时间不超过**
                remain = gamingTimeLeft(account) - seconds;
                if (remain <= 0)
                {
                    return TimeRegulation.Exit;
                }
            }
            else
            {
                return TimeRegulation.Exit;
            }

            return TimeRegulation.None;
        }

        public bool IsHolidy
        {
            get { return _is_holiday; }
        }

        public bool IsBadTime(UserAccount account)
        {
            if (account.AgeRange != UserAccount.AgeLevel.Adult)
            {
                return isRestrictTime();
            }

            return false;
        }

        public bool HasTimeLeft(UserAccount account)
        {
            HLogger.Log("##### HasTimeLeft _last_time : " + _last_time);
            //尝试在登录判定时清除久的时间数据
            _last_time = 0;
            if (account == null || account.Online == null) return true;
            if (account.AgeRange != UserAccount.AgeLevel.Adult)
            {
                return gamingTimeLeft(account) > 70;
            }

            return true;
        }

        private bool isRestrictTime()
        {
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

            HLogger.LogFormat("AntiTimeChecker startAt={0}, endAt={1}", data.segments[0].startAt,
                data.segments[0].endAt);
            HabbyUserClient.Instance.UpdateUserOnlineData(AccountManager.Instance.CurrentAccount, data.segments,
                (response) =>
                {
                    if (response.code == SyncOnlineDataResponse.CODE_SUCCESS)
                    {
                        data.segments.Clear();
                    }
                });
        }

        public enum TimeRegulation : byte
        {
            None = 0,
            Exit
        }
    }
}