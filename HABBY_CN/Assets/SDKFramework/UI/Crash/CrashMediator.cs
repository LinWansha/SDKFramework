using Habby.CNUser;
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
                displayText = string.Format(AntiAddictionDisaplayText.NoRightAge, 8);
                break;
        }

        aligning(alignment);
        UpdateDetailView(displayText);

        OnMediatorHide += AccountManager.Instance.FireCloseNoTime;
        View.btnSure.onClick.AddListener(Close);
    }

    private void UpdateDetailView(string text)
    {
        View.detail.text = text;
        View.detail.resizeTextMinSize = 24;
    }

    private void aligning(float lineSpacing)
    {
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