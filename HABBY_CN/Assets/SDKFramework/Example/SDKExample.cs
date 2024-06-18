using SDKFramework;
using SDKFramework.Account.DataSrc;
using UnityEngine;

public class SDKExample : MonoBehaviour
{
    void Start()
    {
        SDK MRQ = SDK.New();
        MRQ.Run(new SDK.ProcedureOption()
        {
            Splash = () =>
            {
                //HabbyFramework.UI.OpenUI(UIViewID.SplashAdviceUI);
            },
            Login = () =>
            {
                HabbyFramework.UI.OpenUI(UIViewID.EntryUI);
                // HabbyFramework.Account.LoginOrIdentify(new UserAccount()
                // {
                //     LoginChannel = UserAccount.ChannelQQ,
                //     AgeRange = UserAccount.AgeLevel.Adult,
                //     UID = "林万厦",
                //     Age = 21
                // });
            },
            EnterGame = () =>
            {
                //Write your logic for entering the game
                Log.Info("宿主程序进入成功!!!");
            },
        });

        SDK.Procedure.Login();
        
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        webViewObject = new AndroidJavaObject("com.habby.sdk.WebViewPlugin", currentActivity);
#endif
        
#if UNITY_IOS
        _InitWebView(0, 0, Screen.width, Screen.height);
#endif
        Log.Info(WebSource.Config.gameLicenseUrl);
        OpenWeb(WebSource.Config.gameLicenseUrl);
        // OpenWeb(AppSource.WebView.gamePrivacyUrl);
        // OpenWeb(AppSource.WebView.childrenPrivacyUrl);
        // OpenWeb(AppSource.WebView.thirdPartySharingUrl);
        // OpenWeb(AppSource.WebView.personInfoListUrl);
        Invoke(nameof(CloseWeb),5);
    }
    
    
    private AndroidJavaObject webViewObject;
    

    public void OpenWeb(string url)
    {
        Log.Info($"OpenWeb == {url}");
#if UNITY_ANDROID && !UNITY_EDITOR
        webViewObject.Call("LoadURL", url);
#endif
        
#if UNITY_IOS && !UNITY_EDITOR
        _LoadURL(url);
#endif
    }

    public void CloseWeb()
    {
        Log.Info($"CloseWeb");
#if UNITY_ANDROID && !UNITY_EDITOR
        webViewObject.Call("CloseWebView");
#endif
        
#if UNITY_IOS && !UNITY_EDITOR
        _CloseWebView();
#endif
    }
    
    
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void _InitWebView(float x, float y, float width, float height);

    [DllImport("__Internal")]
    private static extern void _LoadURL(string url);

    [DllImport("__Internal")]
    private static extern void _CloseWebView();
#endif
    
    
}