using SDKFramework.Account;
using SDKFramework.UI;
using Sdkhubv2.Runtime.tools;

public class FreeSmsUseUpMediator : UIMediator<FreeSmsUseUpView>
{
    protected override void OnInit()
    {
        base.OnInit();
        View.btnCancel.onClick.AddListener(Close);
        View.btnSend.onClick.AddListener(SendSMSToOperator);
    }

    private void SendSMSToOperator()
    {
        SMSAPIUtil.SendSMS(PhoneLoginInfo.phoneNumber,"6666", (code, msg) =>
        {
            AccountLog.Info($"向运营商发送收费短信 onResult == code：{code} , msg:{msg}");
        }, (errorCode, errorMsg) =>
        {
            AccountLog.Warn($"向运营商发送收费短信 onError == errorCode：{errorCode} , errorMsg:{errorMsg}");
        });
    }
}