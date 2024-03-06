using System;
using UnityEngine;
using Object = UnityEngine.Object;
namespace SDKFramework.Asset
{
    public partial class AssetModule : BaseModule
    {
        private readonly GameObjectPool<GameObjectAsset> gameObjectPool = new GameObjectPool<GameObjectAsset>();
        public GameObject LoadGameObject(string path, Action<GameObject> createNewCallback = null)
        {
            return gameObjectPool.LoadGameObject(path, createNewCallback).gameObject;
        }
        public T LoadGameObject<T>(string path, Action<GameObject> createNewCallback = null) where T : Component
        {
            GameObject go = gameObjectPool.LoadGameObject(path, createNewCallback).gameObject;
            return go.GetComponent<T>();
        }

        public T LoadAssets<T>(string path) where T : Object
        {
            T obj = Resources.Load<T>(path);
            if (obj)
            {
                return obj;
            }
            Debug.LogError($"没有加载到资源：{path}");
            return null;
        }
        
        public void LoadGameObjectAsync(string path, Action<GameObjectAsset> callback, Action<GameObject> createNewCallback = null)
        {
            gameObjectPool.LoadGameObjectAsync(path, callback, createNewCallback);
        }

        public void UnloadCache()
        {
            gameObjectPool.UnloadAllGameObjects();
        }

        public void UnloadGameObject(GameObject go)
        {
            gameObjectPool.UnloadGameObject(go);
        }

        private void UpdateGameObjectRequests()
        {
            gameObjectPool.UpdateLoadRequests();
        }
    }
}
