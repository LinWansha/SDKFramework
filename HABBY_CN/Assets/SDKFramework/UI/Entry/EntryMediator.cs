using Habby.CNUser;
using SDKFramework.UI;
using UnityEngine;
using UnityEngine.UI;
using SDKFramework.Config;
using SDKFramework.Message;

public class EntryMediator : UIMediator<EntryView>
{
    protected override void OnShow(object arg)
    {
        base.OnShow(arg);

        view.ageTip.GetComponent<Image>().sprite =
            HabbyFramework.Asset.LoadAssets<Sprite>("TexTures/" + (int)MessageHandler.appData.applicableRange);

        view.btnEnter.onClick.AddListener(EnterGameOrLogin);
        view.ageTip.onClick.AddListener(ShowAgeTip);
    }

    private void ShowAgeTip()
    {
        HabbyFramework.UI.OpenUI(UIViewID.AgeTipUI);
        HabbyFramework.Message.Post(MessageHandler.appData);
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
        public static AppConfig appData;

        public override void HandleMessage(AppConfig arg)
        {
            appData = arg;
        }
    }
}