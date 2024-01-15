using System.Collections;
using SDKFramework.UI;
using UnityEngine;
using UnityEngine.UI;
//为了尽可能更早的展示健康游戏忠告，不再从UI管理中打开此面板，目前已经从UI管理中弃用
public class SplashAdviceMediator : UIMediator<SplashAdviceView>
{
    internal float fadeInDuration = 1f;

    internal float fadeOutDuration = 1f;

    internal float panelDisplayTime = 5f;

    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
        
        foreach (var VARIABLE in view.texts)
        {
            VARIABLE.gameObject.SetActive(true);
        }
        view.StartCoroutine(DisplayStartupPanel());
    }

    private IEnumerator DisplayStartupPanel()
    {
        // Fade in texts
        float timer = 0f;
        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;

            foreach (Text text in view.texts)
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

            foreach (Text text in view.texts)
            {
                var tempColor = text.color;
                tempColor.a = SmoothStep(1, 0, timer / fadeOutDuration);
                text.color = tempColor;
            }

            yield return null;
        }

        // Deactivate panel
        view.panel.SetActive(false);
        HabbyFramework.UI.OpenUI(UIViewID.EntryUI);
    }

    private float SmoothStep(float start, float end, float t)
    {
        t = Mathf.Clamp01(t);
        t = t * t * (3f - 2f * t);
        return Mathf.Lerp(start, end, t);
    }
}