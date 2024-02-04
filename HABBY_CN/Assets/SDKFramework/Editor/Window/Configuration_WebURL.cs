using SDKFramework.Config;
using UnityEditor;
using UnityEngine;
using System.IO;
namespace SDKFramework.Editor
{
    public class Configuration_WebURL : EditorWindow
    {
        private string gameLicenseUrl = "";
        private string privacyUrl = "";
        private string childrenProtUrl = "";
        private string thirdPartySharingUrl = "";

        // 配置文件路径
        private string streamingAssetsDir = "Assets/StreamingAssets/SDKConfig";
        private string configFileName = "WebConfig.json";

        private GUIStyle headerStyle;
        private const string Tittle = "WebUrl Configuration Linker";
        [MenuItem("SDKFramework/Configuration WebURL")]
        public static void ShowWindow()
        {
            var window =   GetWindow<Configuration_WebURL>(Tittle);
            window.maxSize = new Vector2(600, 250);
            window.minSize = window.maxSize;
        }

        private void OnEnable()
        {
            headerStyle = new GUIStyle
            {
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = new GUIStyleState() { textColor = Color.green }
            };
            // 从配置文件中加载已有链接
            LoadExistingLinks();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            GUILayout.Label(Tittle, headerStyle);
            EditorGUILayout.Space();
            
            gameLicenseUrl = EditorGUILayout.TextField("Game License URL:", gameLicenseUrl);
            privacyUrl = EditorGUILayout.TextField("Privacy URL:", privacyUrl);
            childrenProtUrl = EditorGUILayout.TextField("Children Prot URL:", childrenProtUrl);
            thirdPartySharingUrl = EditorGUILayout.TextField("Third Party Sharing URL:", thirdPartySharingUrl);

            EditorGUILayout.Space();

            if (GUILayout.Button("Save Links"))
            {
                SaveLinks();
            }
        }

        private void LoadExistingLinks()
        {
            string configFilePath = Path.Combine(streamingAssetsDir, configFileName);

            if (File.Exists(configFilePath))
            {
                string jsonContent = File.ReadAllText(configFilePath);
                var configData = JsonUtility.FromJson<WebConfig>(jsonContent);

                gameLicenseUrl = configData.gameLicenseUrl;
                privacyUrl = configData.privacyUrl;
                childrenProtUrl = configData.childrenProtUrl;
                thirdPartySharingUrl = configData.thirdPartySharingUrl;
            }
            else
            {
                Debug.LogError(
                    "Failed to load existing links. Make sure the WebConfig.json file exists and has the correct format.");
            }
        }

        private void SaveLinks()
        {
            var configData = new WebConfig
            {
                gameLicenseUrl = gameLicenseUrl,
                privacyUrl = privacyUrl,
                childrenProtUrl = childrenProtUrl,
                thirdPartySharingUrl = thirdPartySharingUrl
            };

            string jsonContent = JsonUtility.ToJson(configData, true);
            File.WriteAllText(Path.Combine(streamingAssetsDir, configFileName), jsonContent);

            Debug.Log("Links saved to WebConfig.json");
        }
    }

}