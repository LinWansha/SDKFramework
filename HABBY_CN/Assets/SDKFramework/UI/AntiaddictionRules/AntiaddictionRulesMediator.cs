using Habby.CNUser;
using SDKFramework;
using SDKFramework.UI;

public class AntiaddictionRulesMediator : UIMediator<AntiaddictionRulesView>
{
    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
        View.btnSure.onClick.AddListener(Close);
        AccountManager.OnReadedAntiaddtionRules += SDK.Procedure?.EnterGame;
        this.OnMediatorHide += AccountManager.Instance.FireJuvenileEnterGame;
    }

    protected override void OnHide()
    {
        View.btnSure.onClick.RemoveListener(Close);
        base.OnHide();
    }
}