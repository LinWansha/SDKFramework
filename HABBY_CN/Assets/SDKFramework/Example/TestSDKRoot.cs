using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Habby.CNUser;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class TestSDKRoot : MonoBehaviour
{
    void Start()
    {
        AccountManager.Instance.ClearCurrent();
        // HabbyFramework.UI.OpenUI(UIViewID.EntryUI);

    }

    [MenuItem("SDKFramework/Output Persistent Data Folder")] 
    public static void OpenPersistentDataFolder()
    {
        string persistentDataPath = Application.persistentDataPath + "/userHistory";
        HLog.Log("CNUser 账号文件目录: " + persistentDataPath,Color.cyan);
        
    }
}