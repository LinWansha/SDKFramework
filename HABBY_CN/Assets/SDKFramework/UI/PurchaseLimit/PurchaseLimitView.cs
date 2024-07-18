using SDKFramework.UI;
using UnityEngine.UI;

[UIView(typeof(PurchaseLimitMediator), UIViewID.PurchaseLimitUI)]
public class PurchaseLimitView : UIView
{
    public UIText detail;
    public Button btnSure;
}