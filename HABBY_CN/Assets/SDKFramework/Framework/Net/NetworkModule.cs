using System;
using System.Collections;
using UnityEngine.Networking;

namespace SDKFramework.Network
{
    public class NetworkModule : BaseModule
    {
        public void SendRequest(string url, string json, Action<bool, string> callback, int timeOut = 0)
        {
            StartCoroutine(PostRequest(url, json, callback, timeOut));
        }

        public void ReceiveRequest(string url, Action<bool, string> callback, int timeOut = 0)
        {
            StartCoroutine(GetRequest(url, callback, timeOut));
        }


        private IEnumerator PostRequest(string url, string json, Action<bool, string> callback, int timeOut = 0)
        {
            byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(json);

            using (UnityWebRequest request = UnityWebRequest.Post(url, ""))
            {
                request.uploadHandler = new UploadHandlerRaw(postBytes);
                request.downloadHandler = new DownloadHandlerBuffer();
                if (timeOut > 0)
                {
                    request.timeout = timeOut;
                }

                HLogger.Log("=== habby UnityWebRequestPosCN  call url=" + url + " data=" + json + " timeOut=" +
                            timeOut);

                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();

                HLogger.Log("=== habby status code " + request.responseCode);

                if (request.result == UnityWebRequest.Result.ConnectionError ||
                    request.result == UnityWebRequest.Result.DataProcessingError || !request.isDone)
                {
                    HLogger.Log("=== habby UnityWebRequestPostCN  network error");
                    callback?.Invoke(false, string.Empty);
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

                    callback?.Invoke(true, request.downloadHandler.text);
                }
            }
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
    }
}