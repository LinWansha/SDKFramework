using SDKFramework.Config;
using SDKFramework.Message;
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

public class AppSource : MessageHandler<AppConfig>
{
    public static AppConfig Data;

    public override void HandleMessage(AppConfig arg)
    {
        Data = arg;
    }
}