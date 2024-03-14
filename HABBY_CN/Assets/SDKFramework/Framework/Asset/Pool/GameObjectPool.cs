using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDKFramework.Asset
{
    public class GameObjectPool<T> where T : GameObjectPoolAsset
    {
        private readonly Dictionary<int, Queue<T>> gameObjectPool = new Dictionary<int, Queue<T>>();
        private readonly List<GameObjectLoadRequest<T>> requests = new List<GameObjectLoadRequest<T>>();
        private readonly Dictionary<int, GameObject> usingObjects = new Dictionary<int, GameObject>();

        public T LoadGameObject(string path, Action<GameObject> createNewCallback = null)
        {
            int hash = path.GetHashCode();
            if (!gameObjectPool.TryGetValue(hash, out Queue<T> q))
            {
                q = new Queue<T>();
                gameObjectPool.Add(hash, q);
            }
            if (q.Count == 0)
            {
                GameObject prefab = Resources.Load<GameObject>(path);//Addressables.LoadAssetAsync<GameObject>(path).WaitForCompletion();
                GameObject go = UnityEngine.Object.Instantiate(prefab);
                T asset = go.AddComponent<T>();
                createNewCallback?.Invoke(go);
                asset.ID = hash;
                q.Enqueue(asset);
            }

            {
                T asset = q.Dequeue();
                OnGameObjectLoaded(asset);
                return asset;
            }
        }

        public void LoadGameObjectAsync(string path, Action<T> callback, Action<GameObject> createNewCallback = null)
        {
            GameObjectLoadRequest<T> request = new GameObjectLoadRequest<T>(path, callback, createNewCallback);
            requests.Add(request);
        }
        /// <summary>
        /// 卸载(Destroy)对象池中所有对象
        /// </summary>
        public void UnloadAllGameObjects()
        {
            // 先将所有Request加载完毕
            while (requests.Count > 0)
            {
                UpdateLoadRequests();
            }

            // 将所有using Objects 卸载
            if (usingObjects.Count > 0)
            {
                List<int> list = new List<int>();
                foreach (var id in usingObjects.Keys)
                {
                    list.Add(id);
                }
                foreach (var id in list)
                {
                    GameObject obj = usingObjects[id];
                    UnloadGameObject(obj);
                }
            }

            // 将所有缓存清掉
            if (gameObjectPool.Count > 0)
            {
                foreach (var q in gameObjectPool.Values)
                {
                    foreach (var asset in q)
                    {
                        UnityEngine.Object.Destroy(asset.gameObject);
                    }
                    q.Clear();
                }
                gameObjectPool.Clear();
            }
        }
        /// <summary>
        /// 卸载(SetActive(false))单个对象
        /// </summary>
        public void UnloadGameObject(GameObject go)
        {
            if (go == null)
                return;

            T asset = go.GetComponent<T>();
            if (asset == null)
            {
                Debug.LogError($"Unload GameObject失败，找不到GameObjectAsset:{go.name}");
                UnityEngine.Object.Destroy(go);
                return;
            }

            if (!gameObjectPool.TryGetValue(asset.ID, out Queue<T> q))
            {
                q = new Queue<T>();
                gameObjectPool.Add(asset.ID, q);
            }
            q.Enqueue(asset);
            usingObjects.Remove(go.GetInstanceID());
            go.transform.SetParent(ModuleDriver.Instance.GetModule<AssetModule>().releaseObjectRoot);
            go.gameObject.SetActive(false);
        }

        public void UpdateLoadRequests()
        {
            if (requests.Count > 0)
            {
                foreach (var request in requests)
                {
                    int hash = request.Path.GetHashCode();
                    if (!gameObjectPool.TryGetValue(hash, out Queue<T> q))
                    {
                        q = new Queue<T>();
                        gameObjectPool.Add(hash, q);
                    }

                    if (q.Count == 0)
                    {
                        // Addressables.LoadAssetAsync<GameObject>(request.Path).Completed += (obj) =>
                        // {
                        //     GameObject go = UnityEngine.Object.Instantiate(obj.Result);
                        //     T asset = go.AddComponent<T>();
                        //     request.CreateNewCallback?.Invoke(go);
                        //     asset.ID = hash;
                        //     go.SetActive(false);
                        //
                        //     OnGameObjectLoaded(asset);
                        //     request.LoadFinish(asset);
                        // };
                        Resources.LoadAsync<GameObject>(request.Path).Completed((obj) =>
                        {
                            GameObject go = UnityEngine.Object.Instantiate(obj);
                            T asset = go.AddComponent<T>();
                            request.CreateNewCallback?.Invoke(go);
                            asset.ID = hash;
                            
                            OnGameObjectLoaded(asset);
                            request.LoadFinish(asset);
                        });
                    }
                    else
                    {
                        T asset = q.Dequeue();
                        OnGameObjectLoaded(asset);
                        request.LoadFinish(asset);
                    }
                }

                requests.Clear();
            }
        }

        private void OnGameObjectLoaded(T asset)
        {
            asset.gameObject.SetActive(true);
            asset.transform.SetParent(ModuleDriver.Instance.GetModule<AssetModule>().usingObjectRoot);
            int id = asset.gameObject.GetInstanceID();
            usingObjects.Add(id, asset.gameObject);
        }
    }
}
