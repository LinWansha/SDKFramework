using System.Collections;
using System.Collections.Generic;
using Habby.Account;
using Habby.Events;
using Sdkhubv2.Runtime;
using TMPro;
using UnityEngine;

public class ServerRemoteDebugMain : MonoBehaviour
{
    public TMP_Text logText;
    // Start is called before the first frame update
    void Start()
    {
        HabbySDKHubManager.Instance.Init();
        HabbyEventManagerV1.Instance.AddListener<string>("push", ( msg) =>
        {
            SDKHubLog.LogWarning("--- ### unity handleJavaCallback1 msg=" + msg );
            Debug.LogWarning("--- ### unity handleJavaCallback2 msg=" + msg);
            if(logText != null)
            {
                logText.text += msg + "\n";
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleClean()
    {
        if(logText != null)
        {
            logText.text = "";
        }
    }
}
