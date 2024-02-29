using System.Collections;
using SDKFramework.Asset;
using SDKFramework.Config;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// author  :    mengruiqing
/// time    :    2023/12/11
/// function:  CN Version Splash
/// </summary>
public class StartupPanelController : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    [SerializeField] private Text[] texts;

    [SerializeField] private float fadeInDuration = 1f;

    [SerializeField] private float fadeOutDuration = 1f;

    [SerializeField] private float panelDisplayTime = 5f;
    
    private static bool? isNewUser;
    public static bool? IsNewUser
    {
        get => isNewUser;
        set { isNewUser = value; }
    }

    private AppConfig _appdata;

    private void Awake()
    {
        StartCoroutine(UIConfig.DeserializeByFile($"{AssetModule.SDKConfigPath}App.json", (jsonStr) =>
        {
            _appdata = JsonUtility.FromJson<AppConfig>(jsonStr);

            if (_appdata.hasLicense)
            {
                HabbyFramework.UI.OpenUI(UIViewID.EntryUI,_appdata);
                panel.SetActive(false);
            }
            else
            {
                StartCoroutine(DisplayStartupPanel());
            }
            
        }));
    }

    private IEnumerator DisplayStartupPanel()
    {
        // Fade in texts
        float timer = 0f;
        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;

            foreach (Text text in texts)
            {
                var tempColor = text.color;
                tempColor.a = SmoothStep(0, 1, timer / fadeInDuration);
                text.color = tempColor;
            }

            yield return null;
        }

        // Wait for display time
        yield return new WaitForSeconds(panelDisplayTime - fadeInDuration - fadeOutDuration);

        timer = 0f;
        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;

            foreach (Text text in texts)
            {
                var tempColor = text.color;
                tempColor.a = SmoothStep(1, 0, timer / fadeOutDuration);
                text.color = tempColor;
            }

            yield return null;
        }

        // Deactivate panel
        panel.SetActive(false);
        HabbyFramework.UI.OpenUI(UIViewID.EntryUI,_appdata);
        if (isNewUser == true)
        {
            PlayerPrefs.SetInt("IntroVideo", 1);
            SceneManager.LoadSceneAsync("intro");
        }
    }

    private float SmoothStep(float start, float end, float t)
    {
        t = Mathf.Clamp01(t);
        t = t * t * (3f - 2f * t);
        return Mathf.Lerp(start, end, t);
    }
}