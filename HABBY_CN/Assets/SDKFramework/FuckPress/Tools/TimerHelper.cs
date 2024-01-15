using System;
using System.Globalization;
using UnityEngine;

public class TimerHelper
{
    private static TimeSpan correctSpan = TimeSpan.Zero;

    public static void CorrectSysTime(String sysTime)
    {
        CorrectSysTime(Convert.ToDateTime(sysTime));
        // Debug.Log("**********" + lastTime);
    }

    public static void CorrectSysTime(DateTime sysTime)
    {
        correctSpan = sysTime - DateTime.Now;
    }

    public static DateTime GetNowTime()
    {
        return DateTime.Now; //+ correctSpan
    }

    private static TimeSpan timeDistance()
    {
        return DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
    }

    public static int GetTime_Hour()
    {
        return (int)timeDistance().TotalHours;
    }

    public static long GetTime_Sec()
    {
        return (long)timeDistance().TotalSeconds;
        //return DateTime.Now.ToFileTime() / 10000 / 1000;
    }

    public static int GetTime_Day()
    {
        return (int)timeDistance().TotalDays;
    }

    /// <summary>
    /// 格式化成00:00:00
    /// </summary>
    /// <param name="sec">秒</param>
    /// <returns></returns>
    public static string FormatTime(long sec)
    {
        int hour = (int)(sec / 60 / 60);
        int min = (int)(sec / 60 % 60);
        int second = (int)(sec % 60);
        return AddZero(hour) + " : " + AddZero(min) + " : " + AddZero(second);
    }

    private static string AddZero(int a)
    {
        if (a >= 0 && a <= 9)
        {
            return "0" + a.ToString();
        }
        else
        {
            return a.ToString();
        }
    }

    private static string[] datetimePatterns =
    {
        "d.M.yyyy 'г.' H:mm:ss", "d/M/yyyy H:mm:ss", "yyyy/M/d tt hh:mm:ss", "dd.MM.yyyy H:mm:ss",
        "dd-MM-yyyy HH:mm:ss", "dd.MM.yyyy HH:mm:ss", "d/M/yyyy h:mm:ss tt", "M/d/yyyy h:mm:ss tt", "d.M.yyyy H.mm.ss",
        "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy HH:mm:ss", "yyyy. MM. dd. H:mm:ss", "d.M.yyyy HH:mm:ss",
        "dd/MM/yyyy HH:mm:ss", "yyyy/MM/dd H:mm:ss", "yyyy-MM-dd tt h:mm:ss", "d-M-yyyy HH:mm:ss",
        "dd.MM.yyyy HH.mm.ss", "dd.MM.yyyy HH:mm:ss", "dd/MM/yyyy HH:mm:ss", "dd-MM-yyyy HH:mm:ss",
        "dd.MM.yyyy HH:mm:ss", "dd.MM.yyyy H:mm:ss", "d.M.yyyy. H:mm:ss", "dd.MM.yyyy H:mm:ss", "d.M.yyyy HH:mm:ss",
        "yyyy-MM-dd HH:mm:ss", "d.MM.yyyy HH:mm:ss", "dd/MM/yyyy h:mm:ss tt", "dd/MM/yyyy HH.mm.ss",
        "dd.MM.yyyy H:mm:ss", "dd.MM.yy HH:mm:ss", "d. MM. yyyy HH:mm:ss", "dd.MM.yyyy H:mm.ss", "dd.MM.yyyy HH:mm:ss",
        "yyyy-MM-dd HH:mm:ss", "dd.MM.yyyy HH:mm:ss", "dd/MM/yyyy hh:mm:ss tt", "dd/MM/yyyy h:mm:ss tt",
        "dd.MM.yyyy HH:mm:ss", "dd.MM.yyyy HH:mm:ss", "yyyy/MM/dd HH:mm:ss", "d.M.yyyy H:mm:ss", "dd.M.yyyy HH:mm:ss",
        "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd HH:mm:ss",
        "M/d/yyyy h:mm:ss tt", "yyyy-MM-dd h:mm:ss tt", "dd.MM.yyyy HH:mm:ss", "dd-MM-yyyy HH:mm:ss",
        "dd-MM-yyyy HH:mm:ss", "dd/MM/yyyy HH:mm:ss", "yyyy-MM-dd HH:mm:ss", "d/MM/yyyy h:mm:ss tt",
        "dd/MM/yyyy HH:mm:ss", "d-MMM yy HH:mm:ss", "dd/MM/yyyy h:mm:ss tt", "dd.MM.yy 'ý.' HH:mm:ss",
        "yyyy/MM/dd HH:mm:ss", "dd-MM-yy HH.mm.ss", "dd-MM-yy HH:mm:ss", "dd-MM-yy HH:mm:ss", "dd-MM-yyyy HH:mm:ss",
        "dd-MM-yy HH:mm:ss", "dd-MM-yy HH:mm:ss", "dd-MM-yy HH.mm.ss", "dd-MM-yyyy tt h:mm:ss", "dd-MM-yyyy HH:mm:ss",
        "yyyy-MM-dd HH:mm:ss", "yyyy/M/d HH:mm:ss", "dd/MM/yyyy HH:mm:ss", "dd/MM/yy HH:mm:ss", "d/M/yyyy H:mm:ss",
        "dd-MM-yyyy HH:mm:ss", "dd/MM/yyyy HH:mm:ss", "dd-MM-yyyy HH:mm:ss", "yyyy-MM-dd tt h.mm.ss",
        "dd/MM/yyyy h:mm:ss tt", "M/d/yyyy h:mm:ss tt", "dd-MM-yyyy HH:mm:ss", "M/d/yyyy h:mm:ss tt",
        "d/M/yyyy HH:mm:ss", "yyyy-MM-dd HH:mm:ss", "dd.MM.yy HH:mm:ss", "dd-MM-yyyy HH:mm:ss", "dd/MM/yyyy h:mm:ss tt",
        "dd/MM/yyyy h:mm:ss tt", "dd/MM/yyyy h:mm:ss tt", "d/M/yyyy h:mm:ss tt", "dd/MM/yyyy h:mm:ss tt",
        "yyyy/M/d tt h:mm:ss", "yyyy-MM-dd HH:mm:ss", "yyyy-M-d H:mm:ss", "dd/MM/yyyy HH:mm:ss", "dd.MM.yyyy H:mm:ss",
        "yyyy/MM/dd HH:mm:ss", "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy hh:mm:ss tt", "d/M/yyyy H:mm:ss", "yyyy/M/d H:mm:ss",
        "dd.MM.yyyy HH:mm:ss", "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy hh:mm:ss tt", "dd-MM-yy HH:mm:ss",
        "dd.MM.yyyy HH:mm:ss", "d/MM/yyyy H:mm:ss", "dd.MM.yyyy HH:mm:ss", "dd/MM/yyyy HH:mm:ss", "dd.MM.yyyy HH:mm:ss",
        "dd.MM.yyyy H:mm:ss", "dd-MM-yyyy HH:mm:ss", "d/M/yy h:mm:ss tt", "dd.MM.yyyy HH:mm:ss", "d. M. yyyy HH:mm:ss",
        "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd HH:mm:ss", "dd/MM/yyyy HH:mm:ss", "d/MM/yyyy h:mm:ss tt",
        "yyyy/MM/dd HH:mm:ss", "dd-MM-yy HH.mm.ss", "dd-MM-yy h.mm.ss tt", "d-M-yyyy h:mm:ss tt", "yyyy-MM-dd HH:mm:ss",
        "dd/MM/yyyy h:mm:ss tt", "dd/MM/yyyy hh:mm:ss tt", "d/M/yyyy H:mm:ss", "dd.MM.yyyy HH:mm:ss",
        "d/MM/yyyy h:mm:ss tt", "dd/MM/yyyy H:mm:ss", "yyyy-MM-dd HH:mm:ss", "d.M.yyyy H:mm:ss",
        "dd/MM/yyyy hh:mm:ss tt", "d/M/yyyy tt h:mm:ss", "dd.MM.yyyy HH:mm:ss", "yyyy-MM-dd h:mm:ss tt",
        "d/MM/yyyy h:mm:ss tt", "dd.MM.yyyy HH:mm:ss", "dd.MM.yyyy. HH:mm:ss", "dd-MM-yyyy H:mm:ss", "d/M/yyyy H:mm:ss",
        "dd.MM.yyyy HH:mm:ss", "d/MM/yyyy h:mm:ss tt", "d/M/yyyy h:mm:ss tt", "dd/MM/yyyy HH:mm:ss",
        "dd.MM.yyyy. HH:mm:ss", "dd-MM-yyyy H:mm:ss", "dd/MM/yyyy HH:mm:ss", "MM/dd/yyyy h:mm:ss tt",
        "dd/MM/yyyy HH:mm:ss", "d.M.yyyy. HH:mm:ss", "dd-MM-yyyy H:mm:ss", "yyyy/MM/dd h:mm:ss tt",
        "d/M/yyyy h:mm:ss tt", "d.M.yyyy. H:mm:ss", "dd/MM/yyyy hh:mm:ss tt", "d/M/yyyy h:mm:ss tt",
        "d/M/yyyy h:mm:ss tt", "dd/MM/yyyy HH:mm:ss", "d.M.yyyy H:mm:ss", "dd/MM/yyyy hh:mm:ss tt",
        "d/MM/yyyy h:mm:ss tt", "dd/MM/yyyy HH:mm:ss", "d.M.yyyy. HH.mm.ss", "d.M.yyyy H:mm:ss",
        "dd/MM/yyyy hh:mm:ss tt", "dd/MM/yyyy HH:mm:ss", "d/MM/yyyy h:mm:ss tt", "dd/MM/yyyy HH:mm:ss",
        "dd.MM.yyyy. H:mm:ss", "dd/MM/yyyy hh:mm:ss tt", "dd/MM/yyyy h:mm:ss tt", "d/M/yyyy h:mm:ss tt",
        "dd/MM/yyyy HH:mm:ss", "d.M.yyyy. HH.mm.ss", "dd/MM/yyyy hh:mm:ss tt", "d/M/yyyy h:mm:ss tt",
        "d/M/yyyy H:mm:ss", "dd/MM/yyyy HH:mm:ss", "d.M.yyyy. H:mm:ss", "dd/MM/yyyy hh:mm:ss tt",
        "dd/MM/yyyy h:mm:ss tt", "dd-MM-yyyy H:mm:ss", "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy hh:mm:ss tt",
        "d/M/yyyy H:mm:ss", "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy hh:mm:ss tt", "d/M/yyyy h:mm:ss tt",
        "d/M/yyyy h:mm:ss tt", "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy hh:mm:ss tt", "dd-MM-yyyy HH:mm:ss",
        "d/M/yyyy h:mm:ss tt", "d/M/yyyy h:mm:ss tt", "d/M/yyyy h:mm:ss tt", "d/M/yyyy h:mm:ss tt",
        "d/M/yyyy h:mm:ss tt", "d/M/yyyy h:mm:ss tt", "MM/dd/yyyy h:mm:ss tt", "M/d/yyyy h:mm:ss tt",
        "d/M/yyyy h:mm:ss tt", "MM/dd/yyyy HH:mm:ss"
    };

    public static DateTime ParseToDateTime(string s)
    {
        //Debug.LogFormat("TRY TO PARSE TO DATE :: {0}", s);
        if (s == null || s.Length == 0)
            return DateTime.MinValue;

        if (s.Contains("|"))
        {
            s = s.Replace(".", ":");
            try
            {
                return DateTime.ParseExact(s, "yyyy-MM-dd|HH:mm:ss", CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                HLog.LogError(e);
            }
        }

        DateTime d1;
        DateTime d2;
        if (DateTime.TryParse(s, out d1))
        {
        }

        if (DateTime.TryParseExact(s, datetimePatterns, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal,
                out d2))
        {
        }

        if (d1.Equals(DateTime.MinValue) && d2.Equals(DateTime.MinValue))
        {
            if (DateTime.TryParse(s, new CultureInfo("ar-SA"), DateTimeStyles.AdjustToUniversal, out d1)) return d1;
            if (DateTime.TryParse(s, new CultureInfo("th-TH"), DateTimeStyles.AdjustToUniversal, out d1)) return d1;
            return DateTime.MinValue;
        }

        if (!d1.Equals(DateTime.MinValue) && !d2.Equals(DateTime.MinValue))
        {
            if (IsEarlierThan(d2, d1)) return d2;
            else return d1;
        }
        else
        {
            if (!d1.Equals(DateTime.MinValue)) return d1;
            else return d2;
        }
    }

    public static bool IsEarlierThan(DateTime time, DateTime value)
    {
        return time.CompareTo(value) < 0;
    }

    /* 获取 utc 1970-1-1到现在的秒数 */
    public static long Get1970ToNowSeconds()
    {
        return (System.DateTime.UtcNow.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
    }

    /* 获取 utc 1970-1-1到现在的毫秒数 */
    public static long Get1970ToNowMilliseconds()
    {
        return (System.DateTime.UtcNow.ToUniversalTime().Ticks - 621355968000000000) / 10000;
    }

    public static string GetNowUTC2String()
    {
        //暂时没有进行同步的必要
        return System.DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        // return System.DateTime.UtcNow.ToString();
    }

    public static string GetBeforeUTC2String(int seconds)
    {
        //同上
        return System.DateTime.UtcNow.AddSeconds(-seconds).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        // return System.DateTime.UtcNow.AddSeconds(-seconds).ToString();
    }
}