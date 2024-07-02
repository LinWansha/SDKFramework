using System.Collections;
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
        HabbyFramework.Message.Post(new PhoneInfo() { phoneNumber = m_PhoneNum });
        HabbyUserClient.Instance.RequestSmsCode(m_PhoneNum,(response =>
        {
            switch (response.code)
            {
                case 0:
                    AccountLog.Info("发送验证码 成功");
                    View.ActivateWindow(3);
                    RefreshVerifyCodeUI();
                    break;
                case SendUserSmsCodeResponse.CAPTCHA_EXCEEDED_TIMES:        // 超次数
                    AccountLog.Info("验证码发送次数过多，请稍后再试");
                    HabbyFramework.UI.OpenUISingle(UIViewID.FreeSmsUseUpUI);
                    break;
                default:
                    HabbyTextHelper.Instance.ShowTip("发送验证码 失败"+response.code);
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