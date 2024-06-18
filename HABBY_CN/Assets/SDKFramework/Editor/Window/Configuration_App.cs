using SDKFramework.Asset;

namespace SDKFramework.Editor
{
    using System.IO;
    using SDKFramework.Config;
    using UnityEditor;
    using UnityEngine;

    public class Configuration_App : EditorWindow
    {
        private readonly string FilePath = $"{AssetModule.ConfigPath}AppConfig.json";
        private AppConfig _ageTipConfig;

        private Vector2 _scrollPosition;
        private GUIStyle headerStyle;
        private const string Tittle = "Hello HABBY Developer";

        [MenuItem("SDKFramework/Configuration Application")]
        private static void ShowWindow()
        {
            var window = GetWindow<Configuration_App>(Tittle);
            window.maxSize = new Vector2(1600, 800);
            window.minSize = window.maxSize;
        }

        private void OnEnable()
        {
            headerStyle = new GUIStyle
            {
                fontSize = 21,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = new GUIStyleState() { textColor = Color.green }
            };
            Log.Info(FilePath);
            _ageTipConfig = LoadConfiguration();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(Tittle, headerStyle);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Configuration file path: " + FilePath);
            EditorGUILayout.Space();
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            EditorGUILayout.Space();
            _ageTipConfig.hasLicense = EditorGUILayout.Toggle("Has License", _ageTipConfig.hasLicense);
            EditorGUILayout.Space();
            _ageTipConfig.gameName = EditorGUILayout.TextField("Game Name", _ageTipConfig.gameName);
            EditorGUILayout.Space();
            _ageTipConfig.applicableRange =
                (AppConfig.ApplicableRange)EditorGUILayout.EnumPopup("Applicable Range",
                    _ageTipConfig.applicableRange);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("游戏介绍（适龄提示内容）：");
            _ageTipConfig.description =
                EditorGUILayout.TextArea(_ageTipConfig.description, GUILayout.Height(position.height / 2));
            DrawConfirmButton();
        }

        private void DrawConfirmButton()
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Save Configuration", GUILayout.Height(50), GUILayout.Width(1000)))
            {
                SaveConfiguration();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }
        private AppConfig LoadConfiguration()
        {
            if (!File.Exists(FilePath))
            {
                Debug.LogWarning("Configuration file not found! Please ensure the file exists at: " + FilePath);
                return new AppConfig(); // 返回默认对象
            }

            string json = File.ReadAllText(FilePath);
            return JsonUtility.FromJson<AppConfig>(json);
        }

        private void SaveConfiguration()
        {
            string json = JsonUtility.ToJson(_ageTipConfig, true);
            File.WriteAllText(FilePath, json);
            AssetDatabase.Refresh();
            Debug.Log("Configuration saved to: " + FilePath);
        }
    }
}