using SDKFramework.UI;

public class PurchaseRulesMediator : UIMediator<PurchaseRulesView>
{
    protected override void OnInit(PurchaseRulesView view)
    {
        base.OnInit(view);
        view.btnSure.onClick.AddListener(Close);
    }
}