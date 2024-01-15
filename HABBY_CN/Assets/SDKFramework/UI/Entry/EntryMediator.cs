using Habby.CNUser;
using SDKFramework.UI;
using UnityEngine;
using UnityEngine.UI;

public class EntryMediator : UIMediator<EntryView>
{
    protected override void OnInit(EntryView view)
    {
        base.OnInit(view);
        view.btnEnter.onClick.AddListener(() =>
        {
            if (AccountManager.Instance.HasAccount)
            {
                Debug.Log("登录");
                AccountManager.Instance.LoginOrIdentify(AccountManager.Instance.CurrentAccount);
            }
            else
            {
                HabbyFramework.UI.OpenUI(UIViewID.LoginUI);
            }
        });
        view.age12.GetComponent<Button>().onClick.AddListener(() => { HabbyFramework.UI.OpenUI(UIViewID.AgeTipUI); });
    }

    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
    }
}