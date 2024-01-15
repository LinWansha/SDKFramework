using Habby.CNUser;
using SDKFramework.UI;
using UnityEngine;

public class LoginMediator : UIMediator<LoginView>
{
    protected override void OnInit(LoginView view)
    {
        base.OnInit(view);
        Debug.Log("LoginMediator.OnInit");
        view.userIdInput.onValueChanged.AddListener((field) => { });

        view.passwordInput.onValueChanged.AddListener((field) => { });
        view.btnRegister.onClick.AddListener(Register);
        view.btnLogin.onClick.AddListener(Login);
    }

    private void Register()
    {
        if (InputFully(view.userIdInput.text, view.passwordInput.text))
        {
            
            Debug.Log("点击注册");
            //TODO:注册账号
            
        }
        else
        {
            view.noticeLabel.text = "无法创建该用户，请重试"; //todo:封装成事件，松耦合
        }
    }

    private void Login()
    {
        if (InputFully(view.userIdInput.text, view.passwordInput.text))
        {
            Debug.Log("点击登录");
            //TODO:登录账号
            //HabbyFramework.UI.CloseUI(UIViewID.EntryUI);
            AccountManager.Instance.LoginOrIdentify(new UserAccount()
            {
                UID = view.userIdInput.text,
                Password = view.passwordInput.text,
                LoginChannel = "qq"
            });
            Close();
        }
        else
        {
            view.noticeLabel.text = "您输入的用户名或者密码错误";
        }
    }

    private bool InputFully(string uid, string pwd)
    {
        if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(pwd))
        {
            return true;
        }

        return false;
    }

    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
        Debug.Log("LoginMediator.OnShow");
        AccountManager.OnUserNotExists += onUserNotExists;
        AccountManager.OnFailedToCreateUser += faileToCreateUser;
        AccountManager.OnLoginResponseSuccess += onUserExists;
    }

    protected override void OnHide()
    {
        base.OnHide();
        AccountManager.OnUserNotExists += onUserNotExists;
        AccountManager.OnFailedToCreateUser += faileToCreateUser;
        AccountManager.OnLoginResponseSuccess += onUserExists;
    }
    
    private void onUserNotExists()
    {
        view.noticeLabel.gameObject.SetActive(true);
        view.noticeLabel.text = "您输入的用户名或者密码错误";
    }

    private void faileToCreateUser()
    {
        view.noticeLabel.gameObject.SetActive(true);
        view.noticeLabel.text = "无法创建该用户，请重试";
    }

    private void onUserExists()
    {
        Close();
    }
}