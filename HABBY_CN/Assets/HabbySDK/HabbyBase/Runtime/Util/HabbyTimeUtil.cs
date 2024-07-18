using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Habby.Base
{
    public class HabbyTimeUtil
    {
        private static long synServeUTC = 0;
		private static long synLocalUTC = 0;
		private static long synDeltaTime = 0;

		public static long LocalUTC
		{ 
			get
			{
				return HabbtDateUtil.GetLocalTime();
			}
		}
		
		public static long FakeServeUTC 
		{
			get
			{
				return LocalUTC + synDeltaTime;
			} 
		}

		public static DateTime FakeServerTime
		{
			get =>  HabbUtils.TimeStamp2DateTime(FakeServeUTC);
		}

		public static void SynTimestamp(long serverUTC)
		{
			synServeUTC = serverUTC;
			synLocalUTC = LocalUTC;
			
			synDeltaTime = synServeUTC - synLocalUTC;

#if UNITY_EDITOR
			Debug.Log(string.Format("SynTimestamp: serverUTC:{0}, synServeUTC:{1}, synLocalUTC:{2}, synDeltaTime:{3}", serverUTC, synServeUTC, synLocalUTC, synDeltaTime));
#endif
		}
    }
}