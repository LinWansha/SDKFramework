using SDKFramework.UI;
using UnityEngine.UI;

[UIView(typeof(CommonTipMediator), UIViewID.CommonTipUI)]
public class CommonTipView : UIView
{

    public UIText detail;
    public Button btnSure;
}