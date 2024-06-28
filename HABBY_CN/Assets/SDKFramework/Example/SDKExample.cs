using HabbySDK.Sdkhubv2.Runtime.tools;
using SDKFramework;
using SDKFramework.Account.Net;
using SDKFramework.Analytics;
using SDKFramework.Utils.WebView;
using Sdkhubv2.Runtime;
using UnityEngine;

public class SDKExample : MonoBehaviour
{
    void Start()
    {
        HabbySDKHubManager.Instance.Init();    
        
        // HabbyFramework.UI.OpenUI(UIViewID.EntryUI);
        // AnalyticsModule.Instance.Initialization();
        // HabbyFramework.Analytics.InitializeCloud();
        
        HabbyUserClient.Instance.ClearSMSLimit(response =>
        {
                Log.Error("清除手机号（15610937870）的验证码限制成功");
        },"15610937870");

        WebViewBridge.Instance.Init(null);

        // PlatformUtil.isOfficial();
    }
    
}