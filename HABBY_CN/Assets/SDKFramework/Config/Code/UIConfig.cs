using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace SDKFramework.Config
{
    public partial struct UIConfig
    {
        #region File

        // public static void DeserializeByFile(string directory)
        // {
        //     string path = $"{directory}/UIConfig.json";
        //     using (System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read))
        //     {
        //         using (System.IO.StreamReader reader = new System.IO.StreamReader(fs))
        //         {
        //             datas = new List<UIConfig>();
        //             indexMap = new Dictionary<int, int>();
        //             string json = reader.ReadToEnd();
        //             JArray array = JArray.Parse(json);
        //             Count = array.Count;
        //             for (int i = 0; i < array.Count; i++)
        //             {
        //                 JObject dataObject = array[i] as JObject;
        //                 UIConfig data = (UIConfig)dataObject.ToObject(typeof(UIConfig));
        //                 datas.Add(data);
        //                 indexMap.Add(data.ID, i);
        //             }
        //         }
        //     }
        // }
        public static IEnumerator DeserializeByFile(string path, System.Action<string> onComplete)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                yield return DeserializeByWebRequest(path, onComplete);
            }
            else
            {
                DeserializeByReadingLocalFile(path, onComplete);
            }
        }

        private static IEnumerator DeserializeByWebRequest(string path, System.Action<string> onComplete)
        {
            using (UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequest.Get(path))
            {
                Debug.LogError("安卓平台通过UnityWebRequest加载配置");
                yield return request.SendWebRequest();
                if (!request.isNetworkError && !request.isHttpError)
                {
                    ProcessJsonData(request.downloadHandler.text, onComplete);
                }
            }
        }

        private static void DeserializeByReadingLocalFile(string path, System.Action<string> onComplete)
        {
            string json = File.ReadAllText(path);
            ProcessJsonData(json, onComplete);
        }

        private static void ProcessJsonData(string jsonData, System.Action<string> onComplete)
        {
            if (onComplete != null)
            {
                onComplete(jsonData);
            }
            else
            {
                ParseJson(jsonData);
            }
        }

        private static void ParseJson(string json)
        {
            datas = new List<UIConfig>();
            indexMap = new Dictionary<int, int>();
            JArray array = JArray.Parse(json);
            Count = array.Count;
            for (int i = 0; i < array.Count; i++)
            {
                JObject dataObject = array[i] as JObject;
                UIConfig data = (UIConfig)dataObject.ToObject(typeof(UIConfig));
                datas.Add(data);
                indexMap.Add(data.ID, i);
            }
        }

        #endregion
        
        #region Addressable

        public static void DeserializeByAddressable(string directory)
        {
            // string path = $"{directory}/UIConfig.json";
            // UnityEngine.TextAsset ta = Addressables.LoadAssetAsync<UnityEngine.TextAsset>(path).WaitForCompletion();
            // string json = ta.text;
            // datas = new List<UIConfig>();
            // indexMap = new Dictionary<int, int>();
            // JArray array = JArray.Parse(json);
            // Count = array.Count;
            // for (int i = 0; i < array.Count; i++)
            // {
            //     JObject dataObject = array[i] as JObject;
            //     UIConfig data = (UIConfig)dataObject.ToObject(typeof(UIConfig));
            //     datas.Add(data);
            //     indexMap.Add(data.ID, i);
            // }
        }

        #endregion

        #region Bundle

        public static System.Collections.IEnumerator DeserializeByBundle(string directory, string subFolder)
        {
            string bundleName = $"{subFolder}/UIConfig.bytes".ToLower();
            string fullBundleName = $"{directory}/{bundleName}";
            string assetName = $"assets/{bundleName}";
#if UNITY_WEBGL && !UNITY_EDITOR
            UnityEngine.AssetBundle bundle = null;
            UnityEngine.Networking.UnityWebRequest request =
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              UnityEngine.Networking.UnityWebRequestAssetBundle.GetAssetBundle(fullBundleName);
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                UnityEngine.Debug.LogError(request.error);
            }
            else
            {
                bundle = UnityEngine.Networking.DownloadHandlerAssetBundle.GetContent(request);
            }
#else
            yield return null;
            UnityEngine.AssetBundle bundle = UnityEngine.AssetBundle.LoadFromFile($"{fullBundleName}", 0, 0);
#endif
            UnityEngine.TextAsset ta = bundle.LoadAsset<UnityEngine.TextAsset>($"{assetName}");
            string json = ta.text;
            datas = new List<UIConfig>();
            indexMap = new Dictionary<int, int>();
            JArray array = JArray.Parse(json);
            Count = array.Count;
            for (int i = 0; i < array.Count; i++)
            {
                JObject dataObject = array[i] as JObject;
                UIConfig data = (UIConfig)dataObject.ToObject(typeof(UIConfig));
                datas.Add(data);
                indexMap.Add(data.ID, i);
            }
        }

        #endregion

        public static int Count;
        private static List<UIConfig> datas;
        private static Dictionary<int, int> indexMap;

        public static UIConfig ByID(int id)
        {
            if (id <= 0)
            {
                return Null;
            }

            if (!indexMap.TryGetValue(id, out int index))
            {
                throw new System.Exception($"UIConfig找不到ID:{id}");
            }

            return ByIndex(index);
        }

        public static UIConfig ByIndex(int index)
        {
            return datas[index];
        }

        public bool IsNull { get; private set; }
        public static UIConfig Null { get; } = new UIConfig() { IsNull = true };
        public System.Int32 ID { get; set; }
        public string Description { get; set; }
        public string Asset { get; set; }
        public UIMode Mode { get; set; }
    }
}