using System.IO;
using UnityEditor;
using UnityEngine;

public class AgeTipConfigEditorWindow : EditorWindow
{
    private string configFilePath = "Assets/StreamingAssets/SDKConfig/AgeTipConfig.json";
    private AgeTipMediator.AgeTipConfig _ageTipConfig;
    private Vector2 _scrollPosition;

    [MenuItem("SDKFramework/Age Tip Config Editor")]
    private static void ShowWindow()
    {
        GetWindow<AgeTipConfigEditorWindow>("Age Tip Config Editor");
    }

    private void OnEnable()
    {
        _ageTipConfig = LoadConfiguration();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Age Tip Configuration", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Configuration file path: " + configFilePath);
        EditorGUILayout.Space();

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        _ageTipConfig.gameName = EditorGUILayout.TextField("Game Name", _ageTipConfig.gameName);
        _ageTipConfig.applicableRange = (AgeTipMediator.AgeTipConfig.ApplicableRange)EditorGUILayout.EnumPopup("Applicable Range", _ageTipConfig.applicableRange);
        _ageTipConfig.details = EditorGUILayout.TextArea(_ageTipConfig.details, GUILayout.Height(position.height / 2));

        if (GUILayout.Button("Save Configuration"))
        {
            SaveConfiguration();
        }

        EditorGUILayout.EndScrollView();
    }

    private AgeTipMediator.AgeTipConfig LoadConfiguration()
    {
        if (!File.Exists(configFilePath))
        {
            Debug.LogWarning("Configuration file not found! Please ensure the file exists at: " + configFilePath);
            return new AgeTipMediator.AgeTipConfig(); // 返回默认对象
        }

        string json = File.ReadAllText(configFilePath);
        return JsonUtility.FromJson<AgeTipMediator.AgeTipConfig>(json);
    }

    private void SaveConfiguration()
    {
        string json = JsonUtility.ToJson(_ageTipConfig, true);
        File.WriteAllText(configFilePath, json);
        AssetDatabase.Refresh();
        Debug.Log("Configuration saved to: " + configFilePath);
    }
}