using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using SDKFramework.Account.Net;
using UnityEngine;
using UnityEngine.Networking;

namespace SDKFramework.Network
{
    public partial class NetworkModule : BaseModule
    {
#if ENABLE_DEBUG || LOGIN_TEST
        private const string URL_USER_SERVER = "https://test-account-matchmart.lezuan9.com/api/v1/{0}"; //测试
#else
        private const string URL_USER_SERVER = "https://test-account-matchmart.lezuan9.com/api/v1/{0}";// 正式
#endif

        public static string URL => URL_USER_SERVER;

        private static int mPendingMarks = 0;
        
        private static Dictionary<int, float> mPending = new Dictionary<int, float>(2);

        public void Get<TResponse>(string url, Action<TResponse> callback, Action<string> onError)
        {
            StartCoroutine(_Get<TResponse>(url, callback, onError));
        }

        private IEnumerator _Get<TResponse>(string url, Action<TResponse> callback, Action<string> onError)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {

                webRequest.SetRequestHeader("Content-Type", "application/json");
                yield return webRequest.SendWebRequest();

                Log.Info("=== habby status code " + webRequest.responseCode);

                if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                    webRequest.result == UnityWebRequest.Result.DataProcessingError || !webRequest.isDone)
                {
                    Log.Info(webRequest.error);
                    if (onError != null) onError(webRequest.error);
                }
                else
                {
                    TResponse response = JsonConvert.DeserializeObject<TResponse>(webRequest.downloadHandler.text);
                    if (callback != null) callback(response);
                }
            }
        }

        public void Post<T, K>(T requestData, Action<K> callback, string url) where T : Request where K : Response
        {
            string requestStr = JsonConvert.SerializeObject(requestData, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            Log.Info("--- Post=" + url + ",requestStr:" + requestStr);
            StartCoroutine(_Post(url, requestStr, callback));
        }

        private IEnumerator _Post<TResponse>(string path, string requestStr, Action<TResponse> callback)
        {
            int index = addToPending();

            float maxDelay = 45 * mPending.Count;
            while (mPending.ContainsKey(index - 1) && Time.time - mPending[index] < maxDelay)
            {
                HabbyFramework.UI.OpenUISingle(UIViewID.LatencyTimeUI);
                yield return new WaitForSeconds(2);
            }
            string url = string.Format(URL_USER_SERVER, path);
            byte[] bodyRaw = NetEnc.encContent(requestStr);

            using (UnityWebRequest request = UnityWebRequest.Post(url, ""))
            {

                request.timeout = 30;
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                
                Log.Info($"Request url={url} : index={index}, data={requestStr}");
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError ||
                    request.result == UnityWebRequest.Result.DataProcessingError || !request.isDone)
                {
                    string errMsg = request.error;
                    HabbyFramework.UI.OpenUI(UIViewID.LatencyTimeUI);
                    Log.Error($"NetWork Error !!! index={index}, url={url}, msg={errMsg}");
                }
                else
                {
                    Log.Info($"Response index={index}, url={url}, msg={request.responseCode}, content-length={request.downloadedBytes}");
                    try
                    {
                        onRetrieveData(request, callback);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"Response Parse Exception url={url}, index={index}, msg={e.StackTrace}");
                    }
                }

                RemoveLast(index);
            }
            
        }
        
        private void onRetrieveData<TResponse>(UnityWebRequest webRequest, Action<TResponse> callback)
        {
            // string result = Encoding.UTF8.GetString(webRequest.downloadHandler.data);
            string result = NetEnc.decContent(webRequest.downloadHandler.data);
            Log.Info($"response data = {result}");
            TResponse response = JsonConvert.DeserializeObject<TResponse>(result);
            if (callback != null) callback(response);
        }

        public void ClearHttpQueue()
        {
            Log.Info($"Waiting Http Request Queue { mPending.Count}");
            mPendingMarks = 0;
            mPending.Clear();
        }

        private int addToPending()
        {
            int index = ++mPendingMarks;
            mPending.Add(index, Time.time);
            return index;
        }

        private void RemoveLast(int index)
        {
            if (mPending.ContainsKey(index)) mPending.Remove(index);
        }
    }
}