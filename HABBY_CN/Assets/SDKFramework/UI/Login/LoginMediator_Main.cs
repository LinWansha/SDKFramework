using SDKFramework.UI;
using SDKFramework.Utils;

public partial class LoginMediator : UIMediator<LoginView>
{
    protected override void OnInit()
    {
        base.OnInit();

        View.btnAppleLogin.onClick.AddListener(AppleLogin);
        View.btnPhoneLogin.onClick.AddListener(PhoneLogin);
        View.btnWxLogin.onClick.AddListener(WxLogin);
        View.btnQQLogin.onClick.AddListener(QQLogin);
       
        View.btnUserPrivacy.onClick.AddListener(OnShowPrivacyWebView);
        View.btnPersonInfo.onClick.AddListener(OnShowPersonInfoWebView);
        View.btnCallQQGroup.onClick.AddListener(OnCallQQGroup);

        View.btnAppleLogin2.onClick.AddListener(AppleLogin);
        View.btnWxLogin2.onClick.AddListener(WxLogin);
        View.btnQQLogin2.onClick.AddListener(QQLogin);

        View.phoneNumInput.onValueChanged.AddListener(InputPhoneNum);

        View.btnNext.onClick.AddListener(() =>
        {
            View.ActivateWindow(3);
            SendSMSVerificationCode();
        });

        View.btnSend.onClick.AddListener(SendSMSVerificationCode);

        View.inputHandle.onClick.AddListener(View.hideInput.ActivateInputField);

        View.hideInput.onValueChanged.AddListener(HandleCodeInput);
        
    }
    
    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
        View.ActivateWindow(1);
    }

    private void OnCallQQGroup()
    {
        HLogger.Log("jump to qq group");
    }

    private void OnShowPersonInfoWebView()
    {
        HLogger.Log("open web view person info");
    }

    private void OnShowPrivacyWebView()
    {
        HLogger.Log("open web view privacy agreement");
    }
    

    private void QQLogin()
    {
        if (!IsAgreePrivacy()) return;
        HLogger.Log("QQLogin");
    }

    private void WxLogin()
    {
        if (!IsAgreePrivacy()) return;
        HLogger.Log("WxLogin");
    }

    private void PhoneLogin()
    {
        if (!IsAgreePrivacy()) return;
        HLogger.Log("PhoneLogin");
        View.ActivateWindow(2); //这里跳转到手机登录的window 
    }

    private void AppleLogin()
    {
        if (!IsAgreePrivacy()) return;
        HLogger.Log("AppleLogin");
    }

    private bool IsAgreePrivacy()
    {
        if (View.privacyToggle.isOn)
        {
            return true;
        }
        else
        {
            View.noticeText.gameObject.SetActive(true);
            ShowNotice();
            return false;
        }
    }

    private void ShowNotice()
    {
        View.noticeText.SetActive(true);
        CoroutineScheduler.Instance.DrivingBehavior(() => View.noticeText.SetActive(false), 2.5f);
    }
    
}