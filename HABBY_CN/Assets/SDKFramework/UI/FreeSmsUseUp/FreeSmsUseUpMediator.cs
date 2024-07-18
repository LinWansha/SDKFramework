using SDKFramework.Account;
using SDKFramework.UI;
using Sdkhubv2.Runtime.tools;

public class FreeSmsUseUpMediator : UIMediator<FreeSmsUseUpView>
{
    private string uplinkSMS;
    protected override void OnInit()
    {
        base.OnInit();
        View.btnCancel.onClick.AddListener(Close);
        View.btnSend.onClick.AddListener(SendSMSToOperator);
    }

    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
        uplinkSMS = arg as string;
        View.content.text = $"免费验证码短信已用完,\n请编辑短信6666发送到\n{uplinkSMS}，\n即可获得验证码。";
    }

    private void SendSMSToOperator()
    {
        SMSAPIUtil.SendSMS(uplinkSMS,"6666", (code, msg) =>
        {
            AccountLog.Info($"向运营商发送收费短信 onResult == code：{code} , msg:{msg}");
            Close();
        }, (errorCode, errorMsg) =>
        {
            AccountLog.Warn($"向运营商发送收费短信 onError == errorCode：{errorCode} , errorMsg:{errorMsg}");
            Close();
        });
    }
}