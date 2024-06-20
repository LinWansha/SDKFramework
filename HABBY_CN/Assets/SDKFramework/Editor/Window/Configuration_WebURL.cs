using SDKFramework.Config;
using UnityEditor;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using SDKFramework.Asset;

namespace SDKFramework.Editor
{
    public class Configuration_WebURL : EditorWindow
    {
        private string _testAccountServerURL = "";
        private string _prodAccountServerURL = "";
        
        private string _gameLicenseUrl = "";
        private string _gamePrivacyUrl = "";
        private string _childrenPrivacyUrl = "";
        private string _thirdPartySharingUrl = "";
        private string _personInfoListUrl = "";

        // 配置文件路径
        private readonly string FilePath = $"{AssetModule.ConfigPath}WebConfig.json";

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
            Log.Info(FilePath);
            // 从配置文件中加载已有链接
            LoadExistingLinks();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            GUILayout.Label(Tittle, headerStyle);
            EditorGUILayout.Space();
            
            _testAccountServerURL = EditorGUILayout.TextField("Account Server URL (测试) :", _testAccountServerURL);
            _prodAccountServerURL = EditorGUILayout.TextField("Account Server URL (线上):", _prodAccountServerURL);
            
            _gameLicenseUrl = EditorGUILayout.TextField("Game License URL:", _gameLicenseUrl);
            _gamePrivacyUrl = EditorGUILayout.TextField("Game Privacy URL:", _gamePrivacyUrl);
            _childrenPrivacyUrl = EditorGUILayout.TextField("Children Privacy URL:", _childrenPrivacyUrl);
            _thirdPartySharingUrl = EditorGUILayout.TextField("Third Party Sharing URL:", _thirdPartySharingUrl);
            _personInfoListUrl = EditorGUILayout.TextField("Person Info List URL:", _personInfoListUrl);

            EditorGUILayout.Space();

            if (GUILayout.Button("Save Links"))
            {
                SaveLinks();
            }
        }

        private void LoadExistingLinks()
        {

            if (File.Exists(FilePath))
            {
                string jsonContent = File.ReadAllText(FilePath);
                var configData = JsonConvert.DeserializeObject<WebConfig>(jsonContent);

                _testAccountServerURL = configData.AccountServerURL.test;
                _prodAccountServerURL = configData.AccountServerURL.prod;
                
                _gameLicenseUrl = configData.WebView.gameLicenseUrl;
                _gamePrivacyUrl = configData.WebView.gamePrivacyUrl;
                _childrenPrivacyUrl = configData.WebView.childrenPrivacyUrl;
                _thirdPartySharingUrl = configData.WebView.thirdPartySharingUrl;
                _personInfoListUrl = configData.WebView.personInfoListUrl;
                
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
                AccountServerURL =
                {
                    test = _testAccountServerURL,
                    prod = _prodAccountServerURL
                },
                WebView =
                {
                    gameLicenseUrl = _gameLicenseUrl,
                    gamePrivacyUrl = _gamePrivacyUrl,
                    childrenPrivacyUrl = _childrenPrivacyUrl,
                    thirdPartySharingUrl = _thirdPartySharingUrl,
                    personInfoListUrl = _personInfoListUrl
                }
            };

            string jsonContent = JsonConvert.SerializeObject(configData,Formatting.Indented);
            File.WriteAllText(FilePath, jsonContent);

            Debug.Log("Links saved to WebConfig.json");
        }
    }

}