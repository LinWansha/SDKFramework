using SDKFramework.UI;
using UnityEngine;
using UnityEngine.UI;

public class SplashAdviceMediator : UIMediator<SplashAdviceView>
{
    internal float fadeInDuration = 1f;

    internal float fadeOutDuration = 1f;

    internal float panelDisplayTime = 5f;

    private enum State : byte
    {
        FadeIn,
        Wait,
        FadeOut,
        None
    }

    private struct PanelAnimationData
    {
        public State state;
        public float timer;

        public PanelAnimationData(State state, float timer)
        {
            this.state = state;
            this.timer = timer;
        }
    }

    private PanelAnimationData panelData;

    protected override void OnInit()
    {
        base.OnInit();
        panelData = new PanelAnimationData(State.FadeIn, 0f);
    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        switch (panelData.state)
        {
            case State.FadeIn:
                panelData.timer += deltaTime;

                if (panelData.timer >= fadeInDuration)
                {
                    panelData.timer = 0f;
                    panelData.state = State.Wait;
                }
                else
                {
                    UpdateTextsAlpha(SmoothStep(0, 1, panelData.timer / fadeInDuration));
                }

                break;

            case State.Wait:
                panelData.timer += deltaTime;

                if (panelData.timer >= (panelDisplayTime - fadeInDuration - fadeOutDuration))
                {
                    panelData.timer = 0f;
                    panelData.state = State.FadeOut;
                }

                break;

            case State.FadeOut:
                panelData.timer += deltaTime;

                if (panelData.timer >= fadeOutDuration)
                {
                    panelData.state = State.None;
                    Close();
                    HabbyFramework.UI.OpenUI(UIViewID.EntryUI);
                }
                else
                {
                    UpdateTextsAlpha(SmoothStep(1, 0, panelData.timer / fadeOutDuration));
                }

                break;

            case State.None:
            default:
                // No action needed
                break;
        }
    }

    private void UpdateTextsAlpha(float alpha)
    {
        foreach (Text text in View.texts)
        {
            var tempColor = text.color;
            tempColor.a = alpha;
            text.color = tempColor;
        }
    }

    private float SmoothStep(float start, float end, float t)
    {
        t = Mathf.Clamp01(t);
        t = t * t * (3f - 2f * t);
        return Mathf.Lerp(start, end, t);
    }
}