using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SDKFramework.Utils;

namespace UnityEngine
{
    

    /// <summary>
    /// 项目中没有Addressable情况下，
    /// 平替Addressable异步加载的扩展方法
    /// </summary>
    public static class ResourceRequestExt
    {

        private static readonly Dictionary<ResourceRequest, Action<GameObject>> callbacks =
            new Dictionary<ResourceRequest, Action<GameObject>>();

        public static void Completed(this ResourceRequest request, Action<GameObject> successCallback)
        {
            if (request == null) return;

            if (successCallback != null)
            {
                callbacks.Add(request, (asset) =>
                {
                    try
                    {
                        if (asset != null) successCallback(asset);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"[ResourceRequestExt.LoadCompleted] Exception : {ex.Message}\n{ex.StackTrace}");
                    }
                });
            }

            // 异步加载完成时执行的回调函数
            IEnumerator LoadAssetAsync(ResourceRequest req)
            {
                yield return req;
                if (callbacks.ContainsKey(req))
                {
                    callbacks[req](req.asset as GameObject);
                    callbacks.Remove(req);
                }
            }

            AsyncScheduler.Instance.StartCoroutine(LoadAssetAsync(request));
        }

    }
}