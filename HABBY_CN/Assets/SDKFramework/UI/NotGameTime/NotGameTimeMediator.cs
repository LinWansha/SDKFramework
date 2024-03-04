using Habby.CNUser;
using SDKFramework.UI;

public class NotGameTimeMediator : UIMediator<NotGameTimeView>
{
    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
        if (arg!=null)
        {
            HLogger.LogWarning(arg);
            view.contentText.text= arg as string;
        }
        view.btnSure.onClick.AddListener(()=>AccountManager.Instance.FireCloseNoTime());
    }

    protected override void OnHide()
    {
        view.btnSure.onClick.RemoveListener(()=>AccountManager.Instance.FireCloseNoTime());
        base.OnHide();
    }
}