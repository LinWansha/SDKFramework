using SDKFramework;
using SDKFramework.Account;
using SDKFramework.Message;
using SDKFramework.UI;
using SDKFramework.Utils;
using UnityEngine;

public partial class LoginMediator : UIMediator<LoginView>
{
    readonly DecisionMaker loginRunner = new DecisionMaker();
    protected override void OnInit()
    {
        base.OnInit();

        View.btnPhoneLogin.onClick.AddListener(()=>loginRunner.Execute(LoginChannel.Phone));
        View.btnWxLogin.onClick.AddListener(()=>loginRunner.Execute(LoginChannel.WX));
        View.btnWxLogin2.onClick.AddListener(()=>loginRunner.Execute(LoginChannel.WX));
        View.btnQQLogin.onClick.AddListener(()=>loginRunner.Execute(LoginChannel.QQ));
        View.btnQQLogin2.onClick.AddListener(()=>loginRunner.Execute(LoginChannel.QQ));
        View.btnAppleLogin.onClick.AddListener(()=>loginRunner.Execute(LoginChannel.Apple));
        View.btnAppleLogin2.onClick.AddListener(()=>loginRunner.Execute(LoginChannel.Apple));

        View.btnUserPrivacy.onClick.AddListener(OnShowPrivacyWebView);
        View.btnPersonInfo.onClick.AddListener(OnShowPersonInfoWebView);
        View.btnCallQQGroup.onClick.AddListener(OnCallQQGroup);

        View.phoneNumInput.onValueChanged.AddListener(InputPhoneNum);

        View.btnNext.onClick.AddListener(() =>
        {
            View.ActivateWindow(3);
            SendSMSVerificationCode();
        });

        View.btnSend.onClick.AddListener(SendSMSVerificationCode);

        View.inputHandle.onClick.AddListener(View.hideInput.ActivateInputField);

        View.hideInput.onValueChanged.AddListener(HandleCodeInput);

        View.privacyToggle.onValueChanged.AddListener((@agree) =>
        {
            HabbyFramework.Account.SetPrivacyStatus(agree);
        });

        HabbyFramework.Message.Subscribe<MsgType.ShowNoAgreePrivacyNotice>(ShowNotice);
    }

    private void OnCallQQGroup()
    {
        Log.Info("jump to qq group");
    }

    private void OnShowPersonInfoWebView()
    {
        Log.Info("open web view person info");
    }

    private void OnShowPrivacyWebView()
    {
        Log.Info("open web view privacy agreement");
    }

    private void ShowNotice(MsgType.ShowNoAgreePrivacyNotice arg)
    {
        GameObject noticeTextObj = View.noticeText;
        noticeTextObj.SetActive(true);
        CoroutineScheduler.Instance.DelayedInvoke(() => noticeTextObj.SetActive(false), 2.5f);
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
        HabbyFramework.UI.CloseUI(UIViewID.OnClickLoginUI);
        base.OnHide();
    }
}