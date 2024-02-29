using System;
using System.Collections;
using System.Text;
using Habby.CNUser;
using Newtonsoft.Json;
using UnityEngine.Networking;

namespace SDKFramework.Network
{
    public class NetworkModule : BaseModule
    {
        private const string NCAC_URL = "https://ncac-pq.lezuan9.com/api/v1/{0}";
        
        #region POST

        public void SendRequest<TResponse>(Request request, Action<TResponse> callback,  string path)
        {
            string url = string.Format(NCAC_URL, path);
            string json = JsonConvert.SerializeObject(request);
            StartCoroutine(PostRequest(url, json, callback));
        }

        private static void onRetrieveData<TResponse>(UnityWebRequest webRequest, Action<TResponse> callback)
        {
            string result = Encoding.UTF8.GetString(webRequest.downloadHandler.data);
            HLogger.LogFormat("response data = {0}", result);
            TResponse response = JsonConvert.DeserializeObject<TResponse>(result);
            if (callback != null) callback(response);
        }

        private IEnumerator PostRequest<TResponse>(string url, string json, Action<TResponse> callback)
        {
            byte[] postBytes = Encoding.UTF8.GetBytes(json);

            using (UnityWebRequest request = UnityWebRequest.Post(url, ""))
            {
                request.timeout = 30;
                request.uploadHandler = new UploadHandlerRaw(postBytes);
                request.downloadHandler = new DownloadHandlerBuffer();

                HLogger.Log("=== habby UnityWebRequestPosCN  call url=" + url + " data=" + json );

                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();

                HLogger.Log("=== habby status code " + request.responseCode);

                if (request.result == UnityWebRequest.Result.ConnectionError ||
                    request.result == UnityWebRequest.Result.DataProcessingError || !request.isDone)
                {
                    HLogger.LogError("=== habby UnityWebRequestPostCN  network error");
                }
                else
                {
                    HLogger.Log("=== habby UnityWebRequestPostCN  call url=" + url + " code=" + request.responseCode);
                    if (request.downloadHandler == null)
                    {
                        HLogger.Log("=== habby UnityWebRequestPostCN  call url=" + url + " code=" +
                                    request.responseCode +
                                    ",downloadHandler is null");
                    }
                    else
                    {
                        HLogger.Log("=== habby UnityWebRequestPostCN  call url=" + url + " code=" +
                                    request.responseCode +
                                    ",downloadHandler=" + request.downloadHandler.text);
                    }

                    onRetrieveData(request, callback);
                }
            }
        }

        #endregion


        #region GET

        public void ReceiveRequest(string url, Action<bool, string> callback, int timeOut = 0)
        {
            StartCoroutine(GetRequest(url, callback, timeOut));
        }

        private IEnumerator GetRequest(string url, Action<bool, string> callback, int timeOut = 0)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                if (timeOut > 0)
                {
                    request.timeout = timeOut;
                }

                HLogger.Log("=== habby UnityWebRequestGet  call url=" + url + " timeOut=" + timeOut);

                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();

                HLogger.Log("=== habby status code " + request.responseCode);

                if (request.result == UnityWebRequest.Result.ConnectionError ||
                    request.result == UnityWebRequest.Result.DataProcessingError || !request.isDone)
                {
                    HLogger.Log("=== habby UnityWebRequestGet  network error");
                    callback?.Invoke(false, string.Empty);
                }
                else
                {
                    HLogger.Log("=== habby UnityWebRequestGet  call url=" + url + " code=" + request.responseCode);
                    if (request.downloadHandler == null)
                    {
                        HLogger.Log("=== habby UnityWebRequestGet  call url=" + url + " code=" + request.responseCode +
                                    ",downloadHandler is null");
                    }
                    else
                    {
                        HLogger.Log("=== habby UnityWebRequestGet  call url=" + url + " code=" + request.responseCode +
                                    ",downloadHandler=" + request.downloadHandler.text);
                    }

                    callback?.Invoke(true, request.downloadHandler.text);
                }
            }
        }

        #endregion
    }
}