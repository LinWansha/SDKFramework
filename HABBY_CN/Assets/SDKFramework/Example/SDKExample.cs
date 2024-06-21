using SDKFramework;
using SDKFramework.Account.Net;
using SDKFramework.Analytics;
using Sdkhubv2.Runtime;
using UnityEngine;

public class SDKExample : MonoBehaviour
{
    void Start()
    {
        HabbySDKHubManager.Instance.Init();    
        
        HabbyFramework.UI.OpenUI(UIViewID.EntryUI);
        // AnalyticsModule.Instance.Initialization();
        
        HabbyUserClient.Instance.ClearSMSLimit(response =>
        {
                Log.Error("清除手机号（15610937870）的验证码限制成功");
        },"15610937870");
    }
    
}