using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;

/// <summary>
/// 项目中没有Addressable情况下，
/// 平替Addressable异步加载的扩展方法
/// </summary>
public static class ResourceRequestExt
{

    private static readonly Dictionary<ResourceRequest, Action<GameObject>> callbacks = new Dictionary<ResourceRequest, Action<GameObject>>();
    
    public static void Completed(this ResourceRequest request, Action<GameObject> successCallback)
    {
        if (request == null) return;

        if (successCallback != null)
        {
            callbacks.Add(request, ( asset) => {
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
        IEnumerator LoadAssetAsync(ResourceRequest request)
        {
            yield return request;
            if (callbacks.ContainsKey(request))
            {
                callbacks[request](request.asset as GameObject);
                callbacks.Remove(request);
            }
        }

        CoroutineScheduler.Instance.StartCoroutine(LoadAssetAsync(request));
    }
    
}