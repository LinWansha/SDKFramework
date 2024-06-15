using SDKFramework;
using SDKFramework.UI;
using UnityEngine;
using UnityEngine.UI;
using SDKFramework.Utils;

public class EntryMediator : UIMediator<EntryView>
{
    private readonly bool hasAccountHistory = HabbyFramework.Account.AccountHistory.HasAccountHistory;
    
    protected override void OnInit()
    {
        base.OnInit();
        View.versionName.text = $"版本号：{Application.version}";
        if (AppSource.Config.hasLicense)
        {
            if (AppSource.Platform==RuntimePlatform.IPhonePlayer)
            {
                View.privacyLine.SetActive(false);
            }
            else
            {
                View.argeeToggle.isOn = false;//todo: 从本地持久化数据拿取
                View.licenseObj.SetActive(true);
                View.btnPrivacy.onClick.AddListener(() => { });//TODO:open webView
                View.btnPersonalInfo.onClick.AddListener(() => { });
            }
        }
    }

    protected override void OnShow(object arg)
    {
        base.OnShow(arg);

        View.ageTip.GetComponent<Image>().sprite =
            HabbyFramework.Asset.LoadAssets<Sprite>("TexTures/" + (int)AppSource.Config.applicableRange);

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

        HabbyFramework.UI.OpenUI(hasAccountHistory ? UIViewID.QuickLoginUI : UIViewID.LoginUI);
    }

    protected override void OnHide()
    {
        View.btnEnter.onClick.RemoveListener(EnterGameOrLogin);
        View.ageTip.onClick.RemoveListener(ShowAgeTip);
        base.OnHide();
    }

}