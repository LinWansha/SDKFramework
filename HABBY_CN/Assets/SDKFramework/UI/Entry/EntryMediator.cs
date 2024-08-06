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
    
    /// <summary>
    ///  用户协议
    /// </summary>
    public static readonly string KEY_USER_AGREE_USER_AGREEMENT = "USER_AGREE_USER";
    
    protected override void OnInit()
    {
        base.OnInit();
        View.versionName.text = $"版本号：{Application.version}";
        //如果用户已经同意过隐私协议，且云控自动勾选,那么自动勾选
        if (HabbyFramework.Analytics.CloudData.IsPrivacyAgree && PlayerPrefs.GetInt(KEY_USER_AGREE_USER_AGREEMENT) > 0)
        {
            View.privacyToggle.isOn = true;
        }
        else
        {
            View.privacyToggle.isOn = false;
        }
        
        if (Global.Platform==RuntimePlatform.IPhonePlayer)
        {
            View.privacyLine.SetActive(false);
        }
        else
        {
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
            PlayerPrefs.SetInt(KEY_USER_AGREE_USER_AGREEMENT, agree?1:0);
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
        // HabbyFramework.Message.Subscribe<MsgType.RefreshPrivacyToggle>(OnRefreshPrivacyToggle);
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
        View.privacyToggle.onValueChanged.RemoveAllListeners();
        View.btnEnter.onClick.RemoveAllListeners();
        View.ageTip.onClick.RemoveAllListeners();
        View.btnQueryICP.onClick.RemoveAllListeners();
        HabbyFramework.Message.Unsubscribe<MsgType.RefreshPrivacyToggle>(OnRefreshPrivacyToggle);
        base.OnHide();
    }

}