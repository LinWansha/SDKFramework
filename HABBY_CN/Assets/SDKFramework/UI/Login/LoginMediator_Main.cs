using SDKFramework.Account.DataSrc;
using SDKFramework.Account.Net;
using SDKFramework.Account.Utils;
using SDKFramework.Message;
using SDKFramework.UI;
using SDKFramework.Utils;
using UnityEngine;

public partial class LoginMediator : UIMediator<LoginView>
{  
    protected override void OnInit()
    {
        base.OnInit();
        View.btnClose.onClick.AddListener(Close);
        View.btnClose2.onClick.AddListener(Close);
        View.btnClose3.onClick.AddListener(Close);
        
        View.btnBack2.onClick.AddListener(() => View.ActivateWindow(1));
        View.btnBack3.onClick.AddListener(() => View.ActivateWindow(2));

        View.btnAppleLogin.onClick.AddListener(AppleLogin);
        View.btnPhoneLogin.onClick.AddListener(PhoneLogin);
        View.btnWxLogin.onClick.AddListener(WxLogin);
        View.btnQQLogin.onClick.AddListener(QQLogin);
        
        View.btnAppleLogin2.onClick.AddListener(AppleLogin);
        View.btnWxLogin2.onClick.AddListener(WxLogin);
        View.btnQQLogin2.onClick.AddListener(QQLogin);

        View.phoneNumInput.onValueChanged.AddListener(InputPhoneNum);
        
        View.btnNext.onClick.AddListener(() => { View.ActivateWindow(3);
            SendSMSVerificationCode();
        });
        
        View.btnSend.onClick.AddListener(SendSMSVerificationCode);
        
        View.inputHandle.onClick.AddListener(View.hideInput.ActivateInputField);
        
        View.hideInput.onValueChanged.AddListener(HandleCodeInput);
        
        
        
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
        View.ActivateWindow(2);//这里跳转到手机登录的window 
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