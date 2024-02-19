using Habby.CNUser;
using SDKFramework.UI;
using UnityEngine;
using UnityEngine.UI;
using SDKFramework.Config;
using SDKFramework.Message;

public class EntryMediator : UIMediator<EntryView>
{
    protected override void OnInit()
    {
        base.OnInit();
        if (MessageHandler.AppData.hasLicense)
        {
            view.argeeToggle.isOn = false;
            view.licenseObj.SetActive(true);
            view.btnPrivacy.onClick.AddListener(() => { });//TODO:open webview
            view.btnPersonalInfo.onClick.AddListener(() => { });
        }
    }

    protected override void OnShow(object arg)
    {
        base.OnShow(arg);

        view.ageTip.GetComponent<Image>().sprite =
            HabbyFramework.Asset.LoadAssets<Sprite>("TexTures/" + (int)MessageHandler.AppData.applicableRange);

        view.btnEnter.onClick.AddListener(EnterGameOrLogin);
        view.ageTip.onClick.AddListener(ShowAgeTip);
    }

    private void ShowAgeTip()
    {
        HabbyFramework.UI.OpenUI(UIViewID.AgeTipUI);
        HabbyFramework.Message.Post(MessageHandler.AppData);
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

    protected override void OnHide()
    {
        view.btnEnter.onClick.RemoveListener(EnterGameOrLogin);
        view.ageTip.onClick.RemoveListener(ShowAgeTip);
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