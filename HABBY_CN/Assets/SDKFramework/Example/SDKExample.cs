using SDKFramework.Config;
using UnityEngine;

public class SDKExample : MonoBehaviour
{
    private static AppConfig AppData;

    void Awake()
    {
        string jsonStr = Resources.Load<TextAsset>("SDKConfig/App").text;
        AppData = JsonUtility.FromJson<AppConfig>(jsonStr);
        HabbyFramework.Message.Post(AppData);
        HabbyFramework.UI.OpenUI(UIViewID.EntryUI);
    }
}