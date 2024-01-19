using Habby.CNUser;
#if UNITY_EDITOR
#endif
using UnityEditor;
using UnityEngine;

public class TestSDKRoot : MonoBehaviour
{
    void Start()
    {
        AccountManager.Instance.ClearCurrent();
        // HabbyFramework.UI.OpenUI(UIViewID.EntryUI);

    }
#if UNITY_EDITOR
    [MenuItem("SDKFramework/Output Persistent Data Folder")] 
    public static void OpenPersistentDataFolder()
    {
        string persistentDataPath = Application.persistentDataPath + "/userHistory";
        HLog.Log("CNUser 账号文件目录: " + persistentDataPath,Color.cyan);
        
    }
#endif

}