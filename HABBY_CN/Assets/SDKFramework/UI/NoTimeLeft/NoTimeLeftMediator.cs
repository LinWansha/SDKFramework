using SDKFramework.UI;

public class NoTimeLeftMediator : UIMediator<NoTimeLeftView>
{
    protected override void OnInit(NoTimeLeftView view)
    {
        base.OnInit(view);
        view.btnSure.onClick.AddListener(Close);
    }
}