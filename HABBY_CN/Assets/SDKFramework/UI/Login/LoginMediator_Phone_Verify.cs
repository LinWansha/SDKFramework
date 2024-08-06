using System.Collections;
using SDKFramework;
using SDKFramework.Account;
using SDKFramework.Account.Net;
using SDKFramework.Message;
using SDKFramework.UI;
using SDKFramework.Utils;
using UnityEngine;

public partial class LoginMediator : UIMediator<LoginView>
{
    private void SendSMSVerificationCode()
    {
        HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.click_sendcode);
        
        HabbyFramework.UI.OpenUISingle(UIViewID.LatencyTimeUI);
        
        HabbyFramework.Message.Post(new PhoneInfo() { phoneNumber = m_PhoneNum });
        HabbyUserClient.Instance.RequestSmsCode(m_PhoneNum,(response =>
        {
            switch (response.code)
            {
                case 0:
                    HabbyTextHelper.Instance.ShowTip(ErrorMessage.SMS_CODE_SEND_SUCCESS);
                    
                    HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.code_send);
                    View.ActivateWindow(3);
                    RefreshVerifyCodeUI();
                    break;
                case SendUserSmsCodeResponse.CAPTCHA_EXCEEDED_TIMES:
                    HabbyTextHelper.Instance.ShowTip(ErrorMessage.SMS_CODE_SEND_LIMIT);

                    HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.code_limit);
                    HabbyFramework.UI.OpenUISingle(UIViewID.FreeSmsUseUpUI, response.data.uplinkSMS);
                    break;
                default:
                    HabbyTextHelper.Instance.ShowTip(ErrorMessage.SMS_CODE_SEND_FAIL);

                    HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.code_fail);
                    break;
            }

            AccountLog.Info($"RequestSmsCode ResponseCode{response.code}");
        }));
        
    }

    void RefreshVerifyCodeUI()
    {
        View.waitObj.SetActive(true);
        View.btnNext.interactable = false;
        View.btnSend.gameObject.SetActive(false);
        View.showNumText.text = $"    已发送至\n{m_PhoneNum}";
        View.StartCoroutine(StartCountdown(60));
    }
    private IEnumerator StartCountdown(int duration)
    {
        int remainingSeconds = duration;

        while (remainingSeconds > 0)
        {
            View.resendText.text = $"{remainingSeconds}秒后可重发";
            yield return new WaitForSeconds(1);
            remainingSeconds--;
        }

        View.waitObj.SetActive(false);
        View.btnSend.gameObject.SetActive(true);
        View.showNumText.text = "请输入手机验证码";
    }
    
    private int lastLength = 0;
    private void HandleCodeInput(string str)
    {
        var thisLength = str.Length;
        if (lastLength != thisLength)
        {
            if (thisLength == 1 && lastLength == 0)
            {
            }
            lastLength = thisLength;
            if (thisLength == 4)
            {
                HabbyFramework.Message.Post(new PhoneInfo() { phoneNumber = m_PhoneNum, verifyCode = str });
                loginRunner.Execute(LoginChannel.Phone);
            }
        }

        // ReSharper disable once CommentTypo
        //View.btnSend.interactable = str.Length == 4;
    }

}