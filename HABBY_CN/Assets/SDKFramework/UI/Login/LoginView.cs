using System.Collections;
using System.Collections.Generic;
using SDKFramework.UI;
using UnityEngine;
using UnityEngine.UI;

[UIView(typeof(LoginMediator), UIViewID.LoginUI)]
public class LoginView : UIView
{
    public InputField userIdInput;
    public InputField passwordInput;
    public Button btnLogin;
    public Button btnRegister;
    public Text noticeLabel;
}
