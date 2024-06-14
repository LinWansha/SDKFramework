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

    private void HandleCodeInput(string str)
    {
        string inputText = "";
        foreach (char c in str)
        {
            if (char.IsDigit(c))
            {
                inputText += c;
            }
        }

        if (!str.Equals(inputText))
        {
            View.hideInput.text = inputText;
            View.hideInput.caretPosition = inputText.Length; // 设置光标位置
        }

        for (int i = 0; i < View.verifyCodeInput.Count; i++)
        {
            if (i < inputText.Length)
            {
                View.verifyCodeInput[i].text = inputText[i].ToString();
            }
            else
            {
                View.verifyCodeInput[i].text = "";
            }
        }

        if (str.Length==4)
        {
            HabbyFramework.Message.Post(new MsgType.PhoneLogin(){phoneNumber = m_PhoneNum,phoneVerifyCode = str});
        }
    }
}