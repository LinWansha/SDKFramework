using SDKFramework.UI;

public class AntiaddictionRulesMediator : UIMediator<AntiaddictionRulesView>
{
    protected override void OnInit(AntiaddictionRulesView view)
    {
        base.OnInit(view);
        view.btnSure.onClick.AddListener(Close);
    }
}