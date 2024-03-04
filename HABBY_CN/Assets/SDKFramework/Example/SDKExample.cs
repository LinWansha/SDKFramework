using Habby.CNUser;
using SDKFramework.Asset;
using SDKFramework.Config;
using UnityEngine;

public class SDKExample : MonoBehaviour
{
    public static AppConfig AppData;

    void Awake()
    {
        // AccountManager.Instance.ClearCurrent();

        StartCoroutine(UIConfig.DeserializeByFile($"{AssetModule.SDKConfigPath}App.json", (jsonStr) =>
        {
            AppData = JsonUtility.FromJson<AppConfig>(jsonStr);

            HabbyFramework.Message.Post(AppData);
            if (AppData.hasLicense)
            {
               HabbyFramework.UI.OpenUI(UIViewID.EntryUI);
            }
            else
            {
                StartCoroutine(HabbyFramework.UI.OpenUIAsync(UIViewID.SplashAdviceUI));
            }
        }));
    }
}