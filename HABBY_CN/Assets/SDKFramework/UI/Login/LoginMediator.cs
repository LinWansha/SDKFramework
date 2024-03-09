using System;
using Habby.CNUser;
using SDKFramework.Message;
using SDKFramework.UI;
using SDKFramework.Utils;

public class LoginMediator : UIMediator<LoginView>
{
    private string userId;
    private string passward;

    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
        View.btnLogin.onClick.AddListener(Login);
        View.btnRegister.onClick.AddListener(Register);
        HabbyFramework.Message.Subscribe<MsgType.ClosePopup>(ClickMaskCallBack);
    }

    private void ClickMaskCallBack(MsgType.ClosePopup arg)
    {
        if (arg.ViewID==UIViewID.LoginUI)
        {
            Close();
        }
    }


    protected override void OnHide()
    {
        View.btnLogin.onClick.RemoveListener(Login);
        View.btnRegister.onClick.RemoveListener(Register);
        HabbyFramework.Message.Unsubscribe<MsgType.ClosePopup>(ClickMaskCallBack);
        View.userIdInput.text = "";
        View.passwordInput.text = "";
        base.OnHide();
    }

    private void Register()
    {
        if (!InputFully()) return;
        HabbyUserClient.Instance.RegisterWithAccount(userId, passward, (response) =>
        {
            HLogger.Log($"Register Response Code：{response.code}");
            if (response.code == 0)
            {
                HabbyTextHelper.Instance.ShowTip("注册成功！");
                Login();
            }
            else
            {
                HabbyTextHelper.Instance.ShowTip("无法创建该用户，请重试");
            }
        });
    }

    private void Login()
    {
        if (!InputFully()) return;
        HabbyUserClient.Instance.LoginWithAccount(userId, passward, (response) =>
        {
            HLogger.Log($"Login Response Code：{response.code}");
            if (response.code == LoginResponse.CODE_SUCCESS)
            {
                UserAccount account = AccountDataUtil.ParseLoginAccountInfo(response);
                account.LoginChannel = UserAccount.ChannelAccount;
                AccountManager.Instance.LoginOrIdentify(account);
                Close();
            }
            else if (response.code == LoginResponse.CODE_USER_NOT_FOUND)
            {
                HabbyTextHelper.Instance.ShowTip("当前账号尚未注册");
            }
            else
            {
                HabbyTextHelper.Instance.ShowTip("您输入的用户名或者密码错误");
                HLogger.LogError("登录失败");
            }
        });
    }

    private bool InputFully()
    {
        if (!string.IsNullOrEmpty(View.passwordInput.text) && !string.IsNullOrEmpty(View.userIdInput.text))
        {
            userId = View.userIdInput.text;
            passward = View.passwordInput.text;
            return true;
        }

        HabbyTextHelper.Instance.ShowTip("输入的用户名和密码不能为空");
        return false;
    }
}