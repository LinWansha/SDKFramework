using SDKFramework;
using SDKFramework.Account;
using SDKFramework.Account.DataSrc;
using SDKFramework.Message;
using SDKFramework.UI;
using UnityEngine;
using UnityEngine.UI;
using SDKFramework.Utils;
using SDKFramework.Utils.WebView;
using Sdkhubv2.Runtime;
using Sdkhubv2.Runtime.tools;

public class EntryMediator : UIMediator<EntryView>
{
    private AccountModule AccountModule => HabbyFramework.Account;
    
    protected override void OnInit()
    {
        base.OnInit();
        View.versionName.text = $"版本号：{Application.version}";
        if (Global.Platform==RuntimePlatform.IPhonePlayer)
        {
            View.privacyLine.SetActive(false);
        }
        else
        {
            View.privacyToggle.isOn = false;//todo: 从本地持久化数据拿取
            View.btnPrivacy.onClick.AddListener(() =>
            {
                WebViewBridge.Instance.Show(Global.WebView.gamePrivacyUrl);
            });
            View.btnPersonalInfo.onClick.AddListener(() =>
            {
                WebViewBridge.Instance.Show(Global.WebView.personInfoListUrl);
            });
        }
        View.privacyToggle.onValueChanged.AddListener((@agree) =>
        {
            HabbyFramework.Account.SetPrivacyStatus(agree);
        });
        View.btnQueryICP.GetComponentInChildren<UIText>().text = $"备案号可查询链接：{Global.WebView.icpQueryUrl}";
        View.btnQueryICP.onClick.AddListener(() => { Application.OpenURL(Global.WebView.icpQueryUrl); });
    }

    protected override void OnShow(object arg)
    {
        base.OnShow(arg);

        HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.logo_loading_success);

        View.ageTip.GetComponent<Image>().sprite =
            HabbyFramework.Asset.LoadAssets<Sprite>("TexTures/" + (int)Global.App.applicableRange);

        View.btnEnter.onClick.AddListener(EnterGameOrLogin);
        View.ageTip.onClick.AddListener(ShowAgeTip);
        HabbyFramework.Message.Subscribe<MsgType.RefreshPrivacyToggle>(OnRefreshPrivacyToggle);
    }

    private void OnRefreshPrivacyToggle(MsgType.RefreshPrivacyToggle arg)
    {
        View.privacyToggle.isOn = arg.isOn;
    }

    private void ShowAgeTip()
    {
        HabbyFramework.UI.OpenUI(UIViewID.AgeTipUI);
    }

    private void EnterGameOrLogin() //TODO: Review this
    {
        HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.click_startgame_bt);
        if (View.privacyToggle.isOn == false)
        {
            HabbyTextHelper.Instance.ShowTip("请勾选用户协议");
            return;
        }

        if (Global.IsEditor)
        {
            AccountModule.loginRunner.Execute(LoginChannel.Editor);
            return;
        }

        if (BlockingIntoGameMechanics())
            return;
        
        HabbySDKHubManager.Instance.Channel.Login("click start game", (code, msg) =>
        {
            AccountLog.Info($"Login onResult  code :{code},msg  :{msg}");
        }, (errorCode, errorMsg) =>
        {
            AccountLog.Error($"Login onError msg :{errorMsg}");
        });
        
        // var currentAccount = AccountModule.CurrentAccount;
        // if ((!currentAccount.IsLogin && AccountModule.HasAccount) ||
        //     currentAccount.AgeRange != UserAccount.AgeLevel.Adult &&
        //     currentAccount.AgeRange != UserAccount.AgeLevel.Unknown)
        // {
        //     HabbyFramework.UI.OpenUI(UIViewID.QuickLoginUI);
        //     return;
        // }
        //
        // if (!AccountModule.HasAccount)
        // {
        //     if (ShanYanUtil.IsShanYanValid())
        //     {
        //         AccountModule.loginRunner.Execute(LoginChannel.PhoneQuick);
        //     }
        //     else
        //     {
        //         HabbyFramework.UI.OpenUI(UIViewID.LoginUI);
        //     }
        // }
        // else
        //     AccountModule.loginRunner.Execute(LoginChannel.History);
    }
    private bool BlockingIntoGameMechanics()
    {
        if (!Global.CloudData.IsForbidLogin) return false;
        AccountLog.Info($"Blocking user enter game => {Global.CloudData.ForbidLoginNotice}");
        HabbyFramework.UI.OpenUI(UIViewID.CommonTipUI, CommonNoticeType.ForbidLogin);
        return true;
    }
    protected override void OnHide()
    {
        View.btnEnter.onClick.RemoveListener(EnterGameOrLogin);
        View.ageTip.onClick.RemoveListener(ShowAgeTip);
        HabbyFramework.Message.Unsubscribe<MsgType.RefreshPrivacyToggle>(OnRefreshPrivacyToggle);
        base.OnHide();
    }

}