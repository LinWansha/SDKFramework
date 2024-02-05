using SDKFramework.Config;
using SDKFramework.UI;
using UnityEngine;

public class AgeTipMediator : UIMediator<AgeTipView>
{
    protected override void OnShow(object arg)
    {
        base.OnShow(arg);

        view.btnSure.onClick.AddListener(Close);
        HabbyFramework.Message.Subscribe<AppConfig>(OnRefreshAgetip);
        
        //UI不开bestfit就把这个打开
        //view.StartCoroutine(TextDisaplayHelper.GetTextDisaplayHelper().RearrangingText(view.ContentText));
    }

    private void OnRefreshAgetip(AppConfig arg)
    {
        SetMsg(arg.gameName, arg.details);
    }

    public void SetMsg(string gameName, string gameAgeRuleDesc)
    {
        view.Tittle.text = string.Format("《{0}》适龄提示", gameName);
        view.ContentText.text = gameAgeRuleDesc;
    }
    

    protected override void OnHide()
    {
        view.btnSure.onClick.RemoveListener(Close); 
        HabbyFramework.Message.Unsubscribe<AppConfig>(OnRefreshAgetip);
        base.OnHide();
    }
}