using Habby.CNUser;
using SDKFramework.UI;

public class CommonTipMediator : UIMediator<CommonTipView>
{
    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
        view.btnSure.onClick.AddListener(Close);
    }

    protected override void OnHide()
    {
        view.btnSure.onClick.RemoveListener(Close);
        base.OnHide();
    }
}