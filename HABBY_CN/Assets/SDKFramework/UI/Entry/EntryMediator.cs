using Habby.CNUser;
using SDKFramework.UI;
using UnityEngine;
using UnityEngine.UI;
using SDKFramework.Config;

public class EntryMediator : UIMediator<EntryView>
{
    private AppConfig _configData;

    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
        _configData = (AppConfig)arg;
        
        view.ageTip.GetComponent<Image>().sprite =
            HabbyFramework.Asset.LoadAssets<Sprite>("TexTures/" + (int)_configData.applicableRange);
        
        view.btnEnter.onClick.AddListener(EnterGameOrLogin);
        
        view.ageTip.onClick.AddListener(() =>
        {
            HabbyFramework.UI.OpenUI(UIViewID.AgeTipUI);
            HabbyFramework.Message.Post(_configData);
        });
       
    }
    
    private void EnterGameOrLogin()
    {
        if (view.argeeToggle.isOn == false)
        {
            HabbyTextHelper.Instance.ShowTip("请勾选用户协议");
        }
        else
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
        }
    }

}