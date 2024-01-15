using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Networking;

namespace SDKFramework.Net
{
    public class NetModule : BaseModule
    {
        #region netwrok

        public string PayURL
        {
            get
            {
#if ENABLE_DEBUG
                return "http://test-archero-sdk.lezuan7.com/";
#else
                return "http://prod-archero-sdk.lezuan7.com/";
#endif
            }
        }


        private string PreOrder
        {
            get { return "pay/preOrder"; }
        }

        private string CheckResult
        {
            get { return "pay/queryResult"; }
        }

        #endregion

//         public void TryPreOder(Action<bool,RespPreOder> callback,ReqPreOder request)
//         {
//             // Dictionary<string, object> request = new Dictionary<string,  object>();
//             // request["userId"] = GameDataUser.Instance.account.UserID.ToString();
//             // request["clientData"] = GetClientData();
//             String json = JsonConvert.SerializeObject(request); 
// #if ENABLE_DEBUG
//             SDKHubLog.Log($"=== habby TryRequest post url={PayURL + PreOrder} json={json}");
// #endif
//             PostWithAction(PayURL + PreOrder,json,((result, data) =>
//             {
// #if ENABLE_DEBUG
//                 SDKHubLog.Log($"=== habby TryRequest rsp result={result} json={data}");
// #endif
//                 RespPreOder resp = null;
//                 if (result)
//                 {
//                     try
//                     {
//                         resp = JsonConvert.DeserializeObject<RespPreOder>(data);
//                     }
//                     catch (Exception e)
//                     {
//                         SDKHubLog.Log($"=== habby TryPreOder rsp DeserializeObject failed json={data} error={e.Message}");
//                     }
//                 }
//                 callback?.Invoke(result,resp);
//                
//             } ),5);
//         }
//
//         public void TryChecPayResult(Action<bool, RespCheckResult> callback, ReqCheckResult request)
//         {
//             String json = JsonConvert.SerializeObject(request); 
// #if ENABLE_DEBUG
//             SDKHubLog.Log($"=== habby TryChecPayResult post url={PayURL + CheckResult} json={json}");
// #endif
//             PostWithAction(PayURL + CheckResult,json,((result, data) =>
//             {
// #if ENABLE_DEBUG
//                 SDKHubLog.Log($"=== habby TryChecPayResult rsp result={result} json={data}");
// #endif
//                 RespCheckResult resp = null;
//                 if (result)
//                 {
//                     try
//                     {
//                         resp = JsonConvert.DeserializeObject<RespCheckResult>(data);
//                     }
//                     catch (Exception e)
//                     {
//                         SDKHubLog.Log($"=== habby TryChecPayResult rsp DeserializeObject failed json={data} error={e.Message}");
//                         resp = new RespCheckResult();
//                         resp.data = new RespCheckResult.CheckResultData();
//                         resp.data.product_id = request.productId;
//                         callback?.Invoke(false,resp);
//                     }
//
//                     if (resp != null && resp.data != null)
//                     {
//                         resp.data.product_id = request.productId;
//                     }
//                 }
//                 callback?.Invoke(result,resp);
//             } ),10);
//         }


        public void PostWithAction(string url, string json, Action<bool, string> callback, int timeOut = 0)
        {
            CoroutineScheduler.Instance.StartCoroutineCustom(UnityWebRequestPostCN(url, json, callback, timeOut));
        }

        private IEnumerator UnityWebRequestPostCN(string url,string json,Action<bool,string> callback,int timeOut = 0)
        {
            byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(json);
            UnityWebRequest request = new UnityWebRequest(url, "POST");
            request.uploadHandler = new UploadHandlerRaw(postBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.timeout = timeOut;
#if ENABLE_DEBUG
            Debug.Log("=== habby UnityWebRequestPosCN  call url="+url+" data="+json+" timeOut="+timeOut);
#endif

            request.SetRequestHeader("Content-Type","application/json");
            yield return request.SendWebRequest();
#if ENABLE_DEBUG
            Debug.Log("=== habby status code "+request.responseCode);
#endif


            if (request.isHttpError || request.isNetworkError || !request.isDone)
            {
                Debug.Log("=== habby UnityWebRequestPosCN  network error");
                callback?.Invoke(false,string.Empty);
            }
            else
            {
                Debug.Log("=== habby UnityWebRequestPosCN  call url="+url+" code=" + request.responseCode);
                //TGAnalyticsEventWrapper.instance.TrackServerConnect(TGAnalyticsEventWrapper.TGServerConnectType.SUCCESS, url, false, Time.realtimeSinceStartup - connectTime, webRequest.responseCode);
                if(request.downloadHandler == null)
                {
                    Debug.Log("=== habby UnityWebRequestPosCN  call url="+url+" code=" + request.responseCode + ",downloadHandler is null");
                }
                else
                {
                    Debug.Log("=== habby UnityWebRequestPosCN  call url="+url+" code=" + request.responseCode + ",downloadHandler=" + request.downloadHandler.text);
                }
                callback?.Invoke(true,request.downloadHandler.text);
            }
        }
//         private IEnumerator UnityWebRequestPostCN(string url, string json, Action<bool, string> callback,int timeOut = 0)
//         {
//             byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(json);
//
//             // 使用替代方法创建UnityWebRequest
//             using (UnityWebRequest request = UnityWebRequest.Post(url, ""))
//             {
//                 request.uploadHandler = new UploadHandlerRaw(postBytes);
//                 request.downloadHandler = new DownloadHandlerBuffer();
//                 if (timeOut > 0)
//                 {
//                     request.timeout = timeOut;
//                 }
//
// #if ENABLE_DEBUG
//                 Debug.Log("=== habby UnityWebRequestPosCN  call url=" + url + " data=" + json + " timeOut=" + timeOut);
// #endif
//
//                 request.SetRequestHeader("Content-Type", "application/json");
//                 yield return request.SendWebRequest();
//
// #if ENABLE_DEBUG
//                 Debug.Log("=== habby status code " + request.responseCode);
// #endif
//
//                 if (request.result == UnityWebRequest.Result.ConnectionError ||
//                     request.result == UnityWebRequest.Result.DataProcessingError || !request.isDone)
//                 {
//                     Debug.Log("=== habby UnityWebRequestPostCN  network error");
//                     callback?.Invoke(false, string.Empty);
//                 }
//                 else
//                 {
//                     Debug.Log("=== habby UnityWebRequestPostCN  call url=" + url + " code=" + request.responseCode);
//                     //TGAnalyticsEventWrapper.instance.TrackServerConnect(TGAnalyticsEventWrapper.TGServerConnectType.SUCCESS, url, false, Time.realtimeSinceStartup - connectTime, webRequest.responseCode);
//                     if (request.downloadHandler == null)
//                     {
//                         Debug.Log("=== habby UnityWebRequestPostCN  call url=" + url + " code=" + request.responseCode +
//                                   ",downloadHandler is null");
//                     }
//                     else
//                     {
//                         Debug.Log("=== habby UnityWebRequestPostCN  call url=" + url + " code=" + request.responseCode +
//                                   ",downloadHandler=" + request.downloadHandler.text);
//                     }
//
//                     callback?.Invoke(true, request.downloadHandler.text);
//                 }
//             }
//         }
    }
}