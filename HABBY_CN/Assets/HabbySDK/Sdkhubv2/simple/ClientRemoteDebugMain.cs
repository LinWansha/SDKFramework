using System.Collections;
using System.Collections.Generic;
using BestHTTP.WebSocket;
using Sdkhubv2.Runtime;
using Sdkhubv2.Runtime.Platform;
using Sdkhubv2.Runtime.tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClientRemoteDebugMain : MonoBehaviour
{
    public TMP_Text logText;
    public Button loginQQButton;
    public Button cleanLogButton;
    public Button alipayButton;

    #region ui logic
    public void onRemoteQqLogin()
    {
        QQAPIUtil.RequestQqLoginAuthToken(
            (code, msg) =>
            {
                Debug.Log("#onRemoteQqLogin result code:" + code + " msg:" + msg);
            }, (code, msg) =>
            {
                Debug.Log("#onRemoteQqLogin error code:" + code + " msg:" + msg);
            }
        );
    }
    
    public void onAlipayClick()
    {
        HabbySDKHubManager.Instance.PlatformBridge.Pay("{}", (code, msg) =>
        {
            Debug.Log("#onAlipayClick result code:" + code + " msg:" + msg);
        }, (code, msg) =>
        {
            Debug.Log("#onAlipayClick error code:" + code + " msg:" + msg);
        });
    }
    
    
    public void onCLeanLog()
    {
        if (logText != null)
        {
            logText.text = "";
        }
    }

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        HabbySDKHubManager.Instance.Init();
    }

}

