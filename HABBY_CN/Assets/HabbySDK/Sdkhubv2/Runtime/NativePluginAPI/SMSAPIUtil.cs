using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sdkhubv2.Runtime.tools
{
    public class SMSAPIUtil
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phoneNumber">发送给</param>
        /// <param name="content">发送内容</param>
        /// <param name="onResult"></param>
        /// <param name="onError"></param>
        public static void SendSMS(string phoneNumber, string  content, Action<int, string> onResult,Action<int, string> onError)
        {
            try
            {
                string msg = JsonConvert.SerializeObject(new { phone = phoneNumber, content = content });
                HabbySDKHubManager.Instance.PlatformBridge.RequestAsyncFuntion(HabbyApiConst.API_SEND_SMS, msg, (code, msg) =>
                {
                    onResult?.Invoke(code, msg);
                }, (code, msg) =>
                {
                    onError?.Invoke(code, msg);
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                onError?.Invoke(-1,e.StackTrace);
            }
           
        }

    }
}