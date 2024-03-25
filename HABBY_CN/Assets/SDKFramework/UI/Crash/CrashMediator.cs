using SDKFramework;
using SDKFramework.Account.Utils;
using SDKFramework.UI;

public class CrashMediator : UIMediator<CrashView>
{
    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
        if (arg == null) return;

        string displayText = "";
        float alignment = 1f;

        switch ((ExitReason)arg)
        {
            case ExitReason.NoGameTime:
                alignment = 1.1f;
                displayText = AntiAddictionDisaplayText.NoGameTime;
                break;
            case ExitReason.NoTimeLeft:
                View.notice.gameObject.SetActive(true);
                displayText = AntiAddictionDisaplayText.NoTimeLeft;
                break;
            case ExitReason.NoRightAge:
                alignment = 1.3f;
                displayText = string.Format(AntiAddictionDisaplayText.NoRightAge,
                    (int)AppSource.Data.applicableRange);
                break;
        }

        aligning(alignment);
        UpdateDetailView(displayText);

        OnMediatorHide += HabbyFramework.Account.FireCloseNoTime;
        View.btnSure.onClick.AddListener(Close);
    }

    private void UpdateDetailView(string text)
    {
        View.detail.text = text;
    }

    private void aligning(float lineSpacing)
    {
        View.detail.resizeTextMinSize = 24;
        View.detail.lineSpacing = lineSpacing;
    }

    protected override void OnHide()
    {
        View.btnSure.onClick.RemoveListener(Close);
        base.OnHide();
    }
}

public enum ExitReason
{
    NoGameTime,
    NoTimeLeft,
    NoRightAge
}