using System.Collections;
using SDKFramework.Account;
using SDKFramework.Message;
using SDKFramework.UI;
using UnityEngine;

public partial class LoginMediator : UIMediator<LoginView>
{
    private void SendSMSVerificationCode()
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
                HabbyFramework.Message.Post(new SDKEvent.PhoneLogin(){phoneNumber = m_PhoneNum,phoneVerifyCode = str});
            }
        }

        // ReSharper disable once CommentTypo
        //View.btnSend.interactable = str.Length == 4;
    }

}