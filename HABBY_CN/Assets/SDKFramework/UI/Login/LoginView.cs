using System;
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

    public Button inputHandle;

    public List<Text> verifyCodeInput;

    public InputField hideInput;

    public Button btnClose3;

    public Button btnBack3;

    #endregion

    private void Awake()
    {
        if (AppSource.Platform == RuntimePlatform.Android)
        {
            privacyObj.SetActive(false);
            btnAppleLogin.gameObject.SetActive(false);
            btnAppleLogin2.gameObject.SetActive(false);
            btnAppleLogin.GetComponentInParent<HorizontalLayoutGroup>().padding.left = 100;
        }
    }

    /// <summary>
    /// 三个window完全互斥
    /// </summary>
    /// <param name="id">1:main || 2:phone || 3:phone verify</param>
    public void ActivateWindow(int id)
    {
        if (id >= 1 && id <= 3)
        {
            for (int i = 0; i < windows.Count; i++)
            {
                windows[i].SetActive(i == id - 1);
            }
        }
        else
        {
            Debug.LogWarning($"Invalid window ID: {id}. No window will be activated.");
        }

        StopAllCoroutines();
        hideInput.text = "";
        phoneNumInput.text = "";
    }

    public void OnCloseAllWindow()
    {
        HabbyFramework.UI.CloseUI(UIViewID.LoginUI);
    }
}