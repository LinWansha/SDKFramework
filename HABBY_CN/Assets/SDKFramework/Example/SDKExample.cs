using Habby.CNUser;
using SDKFramework.Config;
using UnityEngine;

public class SDKExample : MonoBehaviour
{
    private AppConfig _appdata;

    void Awake()
    {
        // AccountManager.Instance.ClearCurrent();

        StartCoroutine(UIConfig.DeserializeByFile($"{HabbyFramework.Asset.SDKConfigPath}App.json", (jsonStr) =>
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