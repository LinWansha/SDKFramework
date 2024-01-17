using Habby.CNUser;
using SDKFramework.UI;

public class NotGameTimeMediator : UIMediator<NotGameTimeView>
{
    protected override void OnInit(NotGameTimeView view)
    {
        base.OnInit(view);
        view.btnSure.onClick.AddListener(()=>AccountManager.Instance.FireCloseNoTime());
    }
}