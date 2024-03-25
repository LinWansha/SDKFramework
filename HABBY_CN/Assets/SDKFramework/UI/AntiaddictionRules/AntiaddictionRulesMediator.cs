
using SDKFramework;
using SDKFramework.Account;
using SDKFramework.UI;

public class AntiaddictionRulesMediator : UIMediator<AntiaddictionRulesView>
{
    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
        View.btnSure.onClick.AddListener(Close);
        AccountModule.OnReadedAntiaddtionRules += SDK.Procedure?.EnterGame;
        this.OnMediatorHide += HabbyFramework.Account.FireJuvenileEnterGame;
    }

    protected override void OnHide()
    {
        View.btnSure.onClick.RemoveListener(Close);
        base.OnHide();
    }
}