using UnityEngine;

namespace SDKFramework.Utils.WebView
{
    public interface IWebView
    {
        void OpenWebView(string url);
    }

    public class WebViewBridge
    {
        private IWebView _webView;

        private bool Initialized;

        public static WebViewBridge Instance = new WebViewBridge();

        private WebViewBridge() { }
        
        public void Init(IWebView webView)
        {
            if (Initialized)return;
            Log.Warn("WebView Initialized ！！！");
            Initialized = true;
            _webView = webView;
            
        }

        public void Show(string url)
        {
            if (!Initialized)
            {
                Log.Warn("WebView is not Initialized");
                return;
            }
            Application.OpenURL(url);
            // _webView.OpenWebView(url);
            Log.Info($"[WebView] open url: {url}");
        }
    }
}