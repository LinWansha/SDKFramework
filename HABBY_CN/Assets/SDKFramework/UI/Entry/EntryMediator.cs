using Habby.CNUser;
using SDKFramework;
using SDKFramework.UI;
using UnityEngine;
using UnityEngine.UI;
using SDKFramework.Utils;

public class EntryMediator : UIMediator<EntryView>
{
    protected override void OnInit()
    {
        base.OnInit();
        View.versionName.text = $"版本号：{Application.version}";
        if (AppSource.Data.hasLicense)
        {
            View.argeeToggle.isOn = false;
            View.licenseObj.SetActive(true);
            View.btnPrivacy.onClick.AddListener(() => { });//TODO:open webView
            View.btnPersonalInfo.onClick.AddListener(() => { });
        }
    }

    protected override void OnShow(object arg)
    {
        base.OnShow(arg);

        View.ageTip.GetComponent<Image>().sprite =
            HabbyFramework.Asset.LoadAssets<Sprite>("TexTures/" + (int)AppSource.Data.applicableRange);

        View.btnEnter.onClick.AddListener(EnterGameOrLogin);
        View.ageTip.onClick.AddListener(ShowAgeTip);
    }

    private void ShowAgeTip()
    {
        HabbyFramework.UI.OpenUI(UIViewID.AgeTipUI);
    }
    private void EnterGameOrLogin()
    {
        if (View.argeeToggle.isOn == false)
        {
            HabbyTextHelper.Instance.ShowTip("请勾选用户协议");
            return;
        }
        
        if (AccountManager.Instance.HasAccount)
        {
            AccountManager.Instance.LoginOrIdentify(AccountManager.Instance.CurrentAccount);
        }
        else
        {
            HabbyFramework.UI.OpenUI(UIViewID.LoginUI);
        }
    }

    protected override void OnHide()
    {
        View.btnEnter.onClick.RemoveListener(EnterGameOrLogin);
        View.ageTip.onClick.RemoveListener(ShowAgeTip);
        base.OnHide();
    }

    
}