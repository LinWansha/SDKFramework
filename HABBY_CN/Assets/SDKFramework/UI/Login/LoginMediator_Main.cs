using SDKFramework;
using SDKFramework.Account;
using SDKFramework.Analytics;
using SDKFramework.Message;
using SDKFramework.UI;
using SDKFramework.Utils;
using SDKFramework.Utils.WebView;
using Sdkhubv2.Runtime.tools;
using UnityEngine;

public partial class LoginMediator : UIMediator<LoginView>
{
    private readonly DecisionMaker loginRunner = HabbyFramework.Account.loginRunner;

    private CloudData CloudData => HabbyFramework.Analytics.CloudData;
    protected override void OnInit()
    {
        base.OnInit();
        
        View.btnWxLogin.onClick.AddListener(()=>loginRunner.Execute(LoginChannel.WX));
        View.btnWxLogin2.onClick.AddListener(()=>loginRunner.Execute(LoginChannel.WX));
        View.btnQQLogin.onClick.AddListener(()=>loginRunner.Execute(LoginChannel.QQ));
        View.btnQQLogin2.onClick.AddListener(()=>loginRunner.Execute(LoginChannel.QQ));
        View.btnAppleLogin.onClick.AddListener(()=>loginRunner.Execute(LoginChannel.Apple));
        View.btnAppleLogin2.onClick.AddListener(()=>loginRunner.Execute(LoginChannel.Apple));

        View.btnUserPrivacy.onClick.AddListener(OnShowPrivacyWebView);
        View.btnPersonInfo.onClick.AddListener(OnShowPersonInfoWebView);
        View.btnCallQQGroup.onClick.AddListener(OnCallQQGroup);

        View.btnPhoneLogin.onClick.AddListener(() =>
        {
            if (HabbyFramework.Account.CurrentAccount.IsAgreePrivacy)
                View.ActivateWindow(2);
            else
                ShowNotice(default);
        });

        View.btnNext.onClick.AddListener(SendSMSVerificationCode);
        View.btnSend.onClick.AddListener(SendSMSVerificationCode);
        View.phoneNumInput.onValueChanged.AddListener(InputPhoneNum);
        View.verifyCodeInput.OnInputValueChangedEvent += HandleCodeInput;

        View.privacyToggle.onValueChanged.AddListener((@agree) =>
        {
            HabbyFramework.Account.SetPrivacyStatus(agree);
        });

        View.btnBack3.onClick.AddListener(() => { m_PhoneNum = ""; });

        if (!CloudData.IsWxRootOpen || !WeChatAPIUtil.IsInstalled)
        {
            View.btnWxLogin.gameObject.SetActive(false);
            View.btnWxLogin2.gameObject.SetActive(false);
        }

        if (!CloudData.IsQQRootOpen || !QQAPIUtil.IsInstalled)
        {
            View.btnQQLogin.gameObject.SetActive(false);
            View.btnQQLogin2.gameObject.SetActive(false);
        }

        if (!CloudData.IsQQGroupOpen || !QQAPIUtil.IsInstalled)
        {
            View.btnCallQQGroup.gameObject.SetActive(false);
        }
    }

    private void OnCallQQGroup()
    {
        AccountLog.Info("jump to qq group");
        QQAPIUtil.AddQQGroup(CloudData.QQGroupKey);
    }

    private void OnShowPersonInfoWebView()
    {
        AccountLog.Info("open web view person info");
        WebViewBridge.Instance.Show(Global.WebView.personInfoListUrl);
    }

    private void OnShowPrivacyWebView()
    {
        AccountLog.Info("open web view privacy agreement");
        WebViewBridge.Instance.Show(Global.WebView.gamePrivacyUrl);
    }



    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
        if (arg!=null)
        {
            View.ActivateWindow((int)arg);
        }
        HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.login_choose_show);
        HabbyFramework.Message.Subscribe<MsgType.ShowNoAgreePrivacyNotice>(ShowNotice);
        HabbyFramework.Message.Subscribe<MsgType.ResetPrivacyToggle>(OnResetPrivacyToggle);
    }
    private void OnResetPrivacyToggle(MsgType.ResetPrivacyToggle arg)
    {
        View.privacyToggle.isOn = false;
    }
    
    private void ShowNotice(MsgType.ShowNoAgreePrivacyNotice arg)
    {
        if (View.noticeText.activeSelf)return;
        
        GameObject noticeTextObj = View.noticeText;
        noticeTextObj.SetActive(true);
        AsyncScheduler.Instance.DelayedInvoke(() => noticeTextObj.SetActive(false), 2.5f);
    }
    protected override void OnHide()
    {
        View.ActivateWindow(1);
        
        HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.click_webclose);
        HabbyFramework.UI.CloseUI(UIViewID.QuickLoginUI);
        
        HabbyFramework.Message.Unsubscribe<MsgType.ShowNoAgreePrivacyNotice>(ShowNotice);
        HabbyFramework.Message.Unsubscribe<MsgType.ResetPrivacyToggle>(OnResetPrivacyToggle);
        base.OnHide();
    }
}