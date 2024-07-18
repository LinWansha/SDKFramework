using System;
using System.Collections.Generic;
using HabbySDK.Sdkhubv2.Runtime.tools;
using Newtonsoft.Json;

namespace Sdkhubv2.Runtime.tools
{
    public class PayChooseAPIUtil
    {
        /// <summary>
        /// weixin,alipay
        /// </summary>
        /// <param name="payList"></param>
        /// <param name="price"></param>
        /// <param name="onResult"></param>
        /// <param name="onError"></param>
        public static void PayChoose( List<string> payList,int price,Action<int, string> onResult, Action<int, string> onError)
        {
            if(null == payList)
            {
                payList = new List<string>();
                payList.Add("alipay");
            }
            string msg = JsonConvert.SerializeObject(new { paymoney = price,defaultIndex=0,showList = payList });
            HabbySDKHubManager.Instance.PlatformBridge.RequestAsyncFuntion(HabbyApiConst.API_CHOOSE_PAYMENT_TYPE, msg, (code, msg) =>
            {
                onResult?.Invoke(code, msg);
            }, (code, msg) =>
            {
                onError?.Invoke(code, msg);
            });
        }

        public static bool IsSupport
        {
            get
            {
                return PlatformUtil.isOfficial() && PlatformUtil.isAndroid();
            }
        }
        
    }
}