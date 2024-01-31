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

    /// <summary>
    /// 设置提示内容
    /// </summary>
    /// <param name="gameName"> 游戏名字 </param>
    /// <param name="gameAgeRuleDesc">内容描述(每个游戏不一样）</param>
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