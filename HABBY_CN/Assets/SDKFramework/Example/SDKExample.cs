using Habby.CNUser;
using SDKFramework.Asset;
using SDKFramework.Config;
using UnityEngine;

public class SDKExample : MonoBehaviour
{
    private AppConfig _appdata;

    void Awake()
    {
        // AccountManager.Instance.ClearCurrent();

        StartCoroutine(UIConfig.DeserializeByFile($"{AssetModule.SDKConfigPath}App.json", (jsonStr) =>
        {
            _appdata = JsonUtility.FromJson<AppConfig>(jsonStr);

            HabbyFramework.Message.Post(_appdata);
            if (_appdata.hasLicense)
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