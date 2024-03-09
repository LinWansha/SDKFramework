using SDKFramework.UI;

public class PurchaseRulesMediator : UIMediator<PurchaseRulesView>
{
    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
        View.btnSure.onClick.AddListener(Close);
    }

    protected override void OnHide()
    {
        View.btnSure.onClick.RemoveListener(Close);
        base.OnHide();
    }
}