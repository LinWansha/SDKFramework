
using SDKFramework;
using SDKFramework.UI;

public class CommonTipMediator : UIMediator<CommonTipView>
{
    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
        if (!(arg is CommonNoticeType type))
        {
            Log.Warn("CommonTipMediator‘s OnShow params must be enum:(CommonNoticeType)");
            return;
        }
        switch (type)
        {
            case CommonNoticeType.ForbidLogin:
                View.detail.text = Global.CloudData.ForbidLoginNotice;
                break;
            default:
                Log.Warn("CommonNoticeType Error");
                break;
        }
        View.btnSure.onClick.AddListener(Close);
    }

    protected override void OnHide()
    {
        View.btnSure.onClick.RemoveListener(Close);
        base.OnHide();
    }
}

public enum CommonNoticeType : byte
{
    ForbidLogin,
}