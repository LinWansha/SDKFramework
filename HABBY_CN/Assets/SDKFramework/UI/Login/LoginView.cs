using System.Collections.Generic;
using SDKFramework;
using SDKFramework.UI;
using UnityEngine;
using UnityEngine.UI;

[UIView(typeof(LoginMediator), UIViewID.LoginUI)]
public class LoginView : UIView
{
    public List<GameObject> windows;

    #region window_main

    public Button btnAppleLogin;

    public Button btnPhoneLogin;

    public Button btnWxLogin;

    public Button btnQQLogin;

    public GameObject noticeText;
    
    public GameObject privacyObj;

    public Toggle privacyToggle;

    public Button btnUserPrivacy;

    public Button btnPersonInfo;

    public Button btnCallQQGroup;

    public Button btnClose;

    #endregion

    #region window_phoneLogin

    public InputField phoneNumInput;

    public Button btnNext;

    public Button btnAppleLogin2;

    public Button btnWxLogin2;

    public Button btnQQLogin2;

    public Button btnClose2;

    public Button btnBack2;

    #endregion

    #region window_phone_verifiy

    public Text showNumText;

    public GameObject waitObj;

    public Text resendText;

    public Button btnSend;

    public VerifyCodeTool verifyCodeInput;
    
    public Button btnClose3;

    public Button btnBack3;

    #endregion

    private void Awake()
    {
        if (Global.Platform == RuntimePlatform.Android)
        {
            privacyObj.SetActive(false);
            btnAppleLogin.gameObject.SetActive(false);
            btnAppleLogin2.gameObject.SetActive(false);
            // btnAppleLogin.GetComponentInParent<HorizontalLayoutGroup>().padding.left = 100;
        }
    }

    /// <summary>
    /// 三个window完全互斥
    /// </summary>
    /// <param name="id">1:main || 2:phone || 3:phone verify</param>
    public override void ActivateWindow(int id)
    {
        if (id is >= 1 and <= 3)
        {
            for (int i = 0; i < windows.Count; i++)
            {
                windows[i].SetActive(i == id - 1);
            }
        }
        else
        {
            Log.Warn($"Invalid window ID: {id}. No window will be activated.");
        }
        
        StopAllCoroutines();
    }

    public void OnCloseAllWindow()
    {
        HabbyFramework.UI.CloseUI(UIViewID.LoginUI);
    }
}