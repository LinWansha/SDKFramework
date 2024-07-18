using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Habby.Base
{
    public class HabbtDateUtil
    {
        public static DateTime TimeStamp2DateTime(long timeStamp, int timeZone = 0, bool isSecond = true)
        {
            DateTime startTime = new DateTime(1970, 1, 1, timeZone, 0, 0);
            DateTime dt = isSecond
                ? startTime.AddSeconds(timeStamp)
                : startTime.AddMilliseconds(timeStamp);
            return dt;
        }
        
        public static long GetLocalTime()
        {
            TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1);
            long ret = Convert.ToInt64(ts.TotalSeconds);
            return ret;
        }
        
        public static double GetLocalMilliSecTime()
        {
            TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1);
            return ts.TotalMilliseconds;
        }
    }
}