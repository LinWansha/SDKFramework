using SDKFramework;
using SDKFramework.Account;
using SDKFramework.Message;
using SDKFramework.UI;
using SDKFramework.Utils;
using UnityEngine;

public partial class LoginMediator : UIMediator<LoginView>
{
    private readonly DecisionMaker loginRunner = HabbyFramework.Account.loginRunner;
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

        View.btnNext.onClick.AddListener(() =>
        {
            View.ActivateWindow(3);
            SendSMSVerificationCode();
        });

        View.btnPhoneLogin.onClick.AddListener(() =>
        {
            if (HabbyFramework.Account.CurrentAccount.IsAgreePrivacy)
                View.ActivateWindow(2);
            else
                ShowNotice(default);
        });

        View.btnSend.onClick.AddListener(SendSMSVerificationCode);
        View.phoneNumInput.onValueChanged.AddListener(InputPhoneNum);
        View.verifyCodeInput.OnInputValueChangedEvent += HandleCodeInput;

        View.privacyToggle.onValueChanged.AddListener((@agree) =>
        {
            HabbyFramework.Account.SetPrivacyStatus(agree);
        });

        HabbyFramework.Message.Subscribe<SDKEvent.ShowNoAgreePrivacyNotice>(ShowNotice);
    }

    private void OnCallQQGroup()
    {
        AccountLog.Info("jump to qq group");
    }

    private void OnShowPersonInfoWebView()
    {
        AccountLog.Info("open web view person info");
    }

    private void OnShowPrivacyWebView()
    {
        AccountLog.Info("open web view privacy agreement");
    }

    private void ShowNotice(SDKEvent.ShowNoAgreePrivacyNotice arg)
    {
        if (View.noticeText.activeSelf)return;
        
        GameObject noticeTextObj = View.noticeText;
        noticeTextObj.SetActive(true);
        AsyncScheduler.Instance.DelayedInvoke(() => noticeTextObj.SetActive(false), 2.5f);
    }

    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
        if (arg!=null)
        {
            View.ActivateWindow((int)arg);
        }
    }

    protected override void OnHide()
    {
        View.ActivateWindow(1);
        HabbyFramework.UI.CloseUI(UIViewID.QuickLoginUI);
        base.OnHide();
    }
}