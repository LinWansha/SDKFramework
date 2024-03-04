using Habby.CNUser;
using SDKFramework.UI;

public class NoTimeLeftMediator : UIMediator<NoTimeLeftView>
{
    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
        view.btnSure.onClick.AddListener(AccountManager.Instance.FireCloseNoTime);
    }

    protected override void OnHide()
    {
        view.btnSure.onClick.RemoveListener(AccountManager.Instance.FireCloseNoTime);
        base.OnHide();
    }
}