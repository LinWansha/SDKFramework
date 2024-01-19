namespace SDKFramework.Editor
{
    using System.IO;
    using SDKFramework.Config;
    using UnityEditor;
    using UnityEngine;

    public class Configuration_AgeTip : EditorWindow
    {
        private string configFilePath = "Assets/StreamingAssets/SDKConfig/AgeTipConfig.json";
        private AgeTipConfig _ageTipConfig;
        
        private Vector2 _scrollPosition;
        private GUIStyle headerStyle;
        private const string Tittle = "AgeTip Configuration Linker";
        
        [MenuItem("SDKFramework/Configuration Age Tip")]
        private static void ShowWindow()
        {
            var window =GetWindow<Configuration_AgeTip>(Tittle);
            window.maxSize = new Vector2(1800, 800);
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
            _ageTipConfig = LoadConfiguration();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(Tittle, headerStyle);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Configuration file path: " + configFilePath);
            EditorGUILayout.Space();

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            _ageTipConfig.gameName = EditorGUILayout.TextField("Game Name", _ageTipConfig.gameName);
            _ageTipConfig.applicableRange =
                (AgeTipConfig.ApplicableRange)EditorGUILayout.EnumPopup("Applicable Range",
                    _ageTipConfig.applicableRange);
            _ageTipConfig.details =
                EditorGUILayout.TextArea(_ageTipConfig.details, GUILayout.Height(position.height / 2));

            if (GUILayout.Button("Save Configuration"))
            {
                SaveConfiguration();
            }

            EditorGUILayout.EndScrollView();
        }

        private AgeTipConfig LoadConfiguration()
        {
            if (!File.Exists(configFilePath))
            {
                Debug.LogWarning("Configuration file not found! Please ensure the file exists at: " + configFilePath);
                return new AgeTipConfig(); // 返回默认对象
            }

            string json = File.ReadAllText(configFilePath);
            return JsonUtility.FromJson<AgeTipConfig>(json);
        }

        private void SaveConfiguration()
        {
            string json = JsonUtility.ToJson(_ageTipConfig, true);
            File.WriteAllText(configFilePath, json);
            AssetDatabase.Refresh();
            Debug.Log("Configuration saved to: " + configFilePath);
        }
    }
}