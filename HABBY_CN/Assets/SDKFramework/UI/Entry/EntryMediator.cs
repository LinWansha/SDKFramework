using SDKFramework;
using SDKFramework.Account;
using SDKFramework.Account.DataSrc;
using SDKFramework.Account.Net;
using SDKFramework.Message;
using SDKFramework.UI;
using UnityEngine;
using UnityEngine.UI;
using SDKFramework.Utils;
using SDKFramework.Utils.WebView;

public class EntryMediator : UIMediator<EntryView>
{
    private AccountModule AccountModule = HabbyFramework.Account;
    
    protected override void OnInit()
    {
        base.OnInit();
        View.versionName.text = $"版本号：{Application.version}";
        if (Global.App.hasLicense)
        {
            if (Global.Platform==RuntimePlatform.IPhonePlayer)
            {
                View.privacyLine.SetActive(false);
            }
            else
            {
                View.privacyToggle.isOn = false;//todo: 从本地持久化数据拿取
                View.licenseObj.SetActive(true);
                View.btnPrivacy.onClick.AddListener(() =>
                {
                    WebViewBridge.Instance.Show(Global.WebView.gamePrivacyUrl);
                });
                View.btnPersonalInfo.onClick.AddListener(() =>
                {
                    WebViewBridge.Instance.Show(Global.WebView.personInfoListUrl);
                });
            }
        }
        View.privacyToggle.onValueChanged.AddListener((@agree) =>
        {
            HabbyFramework.Account.SetPrivacyStatus(agree);
        });
    }

    protected override void OnShow(object arg)
    {
        base.OnShow(arg);

        HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.logo_loading_success);

        View.ageTip.GetComponent<Image>().sprite =
            HabbyFramework.Asset.LoadAssets<Sprite>("TexTures/" + (int)Global.App.applicableRange);

        View.btnEnter.onClick.AddListener(EnterGameOrLogin);
        View.ageTip.onClick.AddListener(ShowAgeTip);
        HabbyFramework.Message.Subscribe<MsgType.ResetPrivacyToggle>(OnResetPrivacyToggle);
    }

    private void OnResetPrivacyToggle(MsgType.ResetPrivacyToggle arg)
    {
        View.privacyToggle.isOn = false;
    }

    private void ShowAgeTip()
    {
        HabbyFramework.UI.OpenUI(UIViewID.AgeTipUI);
    }
    private void EnterGameOrLogin()
    {
        HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.click_startgame_bt);
        if (View.privacyToggle.isOn == false)
        {
            HabbyTextHelper.Instance.ShowTip("请勾选用户协议");
            return;
        }

        if (Global.IsEditor)
        {
            AccountModule.loginRunner.Execute(HabbyFramework.Account.LoginMethodMap["editor"]);
            return;
        }

        if ((AccountModule.IsLoginStateDirty && AccountModule.HasAccount) ||
            AccountModule.CurrentAccount.AgeRange != UserAccount.AgeLevel.Adult &&
            AccountModule.CurrentAccount.AgeRange != UserAccount.AgeLevel.Unknown)
        {
            HabbyFramework.UI.OpenUI(UIViewID.QuickLoginUI);
            return;
        }
        
        if (!AccountModule.HasAccount)
            HabbyFramework.UI.OpenUI(UIViewID.LoginUI);
        else
            // AccountModule.loginRunner.Execute(HabbyFramework.Account.LoginMethodMap[Global.Channel]);
            HabbyUserClient.Instance.LoginWithToken((response) =>
            {
                switch (response.code)
                {
                    case Response.CODE_SUCCESS:
                        HabbyFramework.Account.RealNameLogin((success) =>
                        {
                            AccountLog.Info(success ? "RealNameLogin Success" : "RealNameLogin Failed");
                        });
                        break;
                    case Response.CODE_APP_TOKEN_EXPIRE:
                        HabbyTextHelper.Instance.ShowTip($"{Global.Channel}  授权过期,请重新授权");
                        break;
                    case Response.CAPTCHA_INVALID:
                        break;
                }
            },Global.Channel, HabbyFramework.Account.CurrentAccount.AccessToken);
    }

    protected override void OnHide()
    {
        View.btnEnter.onClick.RemoveListener(EnterGameOrLogin);
        View.ageTip.onClick.RemoveListener(ShowAgeTip);
        HabbyFramework.Message.Unsubscribe<MsgType.ResetPrivacyToggle>(OnResetPrivacyToggle);
        base.OnHide();
    }

}