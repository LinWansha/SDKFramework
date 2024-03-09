using Habby.CNUser;
using SDKFramework.UI;
using UnityEngine;
using UnityEngine.UI;
using SDKFramework.Config;
using SDKFramework.Message;
using SDKFramework.Utils;

public class EntryMediator : UIMediator<EntryView>
{
    protected override void OnInit()
    {
        base.OnInit();
        if (MessageHandler.AppData.hasLicense)
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
            HabbyFramework.Asset.LoadAssets<Sprite>("TexTures/" + (int)MessageHandler.AppData.applicableRange);

        View.btnEnter.onClick.AddListener(EnterGameOrLogin);
        View.ageTip.onClick.AddListener(ShowAgeTip);
    }

    private void ShowAgeTip()
    {
        HabbyFramework.UI.OpenUI(UIViewID.AgeTipUI);
        HabbyFramework.Message.Post(MessageHandler.AppData);
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

    private class MessageHandler : MessageHandler<AppConfig>
    {
        public static AppConfig AppData;

        public override void HandleMessage(AppConfig arg)
        {
            AppData = arg;
        }
    }
}