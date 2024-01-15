using SDKFramework.UI;
using UnityEngine;
using UnityEngine.UI;

[UIView(typeof(LoginSuccessMediator), UIViewID.LoginSuccessUI)]
public class LoginSuccessView : UIView
{
    public GameObject weixin;
    public GameObject qq;
    public GameObject phone;
    public GameObject apple;
    
    public Text nameText;

    public RectTransform root;

}