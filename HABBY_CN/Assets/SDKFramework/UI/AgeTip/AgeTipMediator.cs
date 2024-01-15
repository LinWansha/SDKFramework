using SDKFramework.UI;

public class AgeTipMediator : UIMediator<AgeTipView>
{
    protected override void OnInit(AgeTipView view)
    {
        base.OnInit(view);
        view.btnSure.onClick.AddListener(Close);
    }

    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
        //UI不开bestfit就把这个打开
        view.StartCoroutine(TextDisaplayHelper.GetTextDisaplayHelper().RearrangingText(view.ContentText));

    }
    
    /// <summary>
    /// 设置提示内容
    /// </summary>
    /// <param name="gameName"> 游戏名字 </param>
    /// <param name="gameAgeRuleDesc">内容描述(每个游戏不一样）</param>
    public void SetMsg(string gameName,string gameAgeRuleDesc)
    {
        view.Tittle.text = string.Format("《{0}》适龄提示",gameName);
        view.ContentText.text = gameAgeRuleDesc;
    }
}