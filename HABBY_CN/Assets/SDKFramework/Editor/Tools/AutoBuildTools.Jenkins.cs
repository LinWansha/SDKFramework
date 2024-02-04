using System.Diagnostics;
using UnityEngine;
using UnityEditor;

public class BuildUnityProject
{
    static string[] Scenes = { "Assets/SDKFramework/Example/SDKExample.unity" }; // 替换为您实际的场景
    static string EXPORT_FOLDER = "D:\\UnityWork\\SDKFramework\\HABBY_CN\\AndroidProject"; // 您要导出的 Android Studio 项目的文件夹名

    [MenuItem("SDKFramework/Jenkins Trigger")]
    public static void PerformAndroidBuild()
    {
        BuildPipeline.BuildPlayer(Scenes, EXPORT_FOLDER, BuildTarget.Android,
            BuildOptions.AcceptExternalModificationsToPlayer);
    }
    
    [MenuItem("SDKFramework/Open PersistentDataPath")]
    public static void OpenDirectory()
    {
        string directoryPath = Application.persistentDataPath;
        
        try
        {
            Process.Start(directoryPath);
        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.LogError("Error opening directory: " + ex.Message);
        }
    }
}