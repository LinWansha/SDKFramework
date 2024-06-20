using SDKFramework;
using SDKFramework.UI;

public class AgeTipMediator : UIMediator<AgeTipView>
{
    protected override void OnShow(object arg)
    {
        base.OnShow(arg);

        View.btnSure.onClick.AddListener(Close);
        SetMsg(Global.App.gameName, Global.App.description);

        //UI不开bestfit就把这个打开
        //view.StartCoroutine(TextDisaplayHelper.GetTextDisaplayHelper().RearrangingText(view.ContentText));
    }

    public void SetMsg(string gameName, string gameAgeRuleDesc)
    {
        View.Tittle.text = string.Format("《{0}》适龄提示", gameName);
        View.ContentText.text = gameAgeRuleDesc;
    }
    
    protected override void OnHide()
    {
        View.btnSure.onClick.RemoveListener(Close); 
        base.OnHide();
    }
}