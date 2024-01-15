using Habby.CNUser;
using SDKFramework.UI;

public class CommonTipMediator : UIMediator<CommonTipView>
{
    protected override void OnInit(CommonTipView view)
    {
        base.OnInit(view);
        view.btnSure.onClick.AddListener(Close);
    }
    
}