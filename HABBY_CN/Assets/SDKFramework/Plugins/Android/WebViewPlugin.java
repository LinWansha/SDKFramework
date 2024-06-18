package com.habby.sdk;

import android.os.Handler;
import android.os.Looper;
import android.app.Activity;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.LinearLayout;

public class WebViewPlugin {

    private Activity activity;
    private WebView webView;
    private Handler handler;

    public WebViewPlugin(Activity activity) {
        this.activity = activity;
        this.handler = new Handler(Looper.getMainLooper());
    }

    public void LoadURL(String url) {
        handler.post(new Runnable() {
            @Override
            public void run() {
                if (webView == null) {
                    webView = new WebView(activity);
                    webView.setWebViewClient(new WebViewClient());
                    webView.getSettings().setJavaScriptEnabled(true);

                    LinearLayout layout = new LinearLayout(activity);
                    layout.addView(webView);
                    activity.setContentView(layout);
                }
                webView.loadUrl(url);
            }
        });
    }

    public void CloseWebView() {
        handler.post(new Runnable() {
            @Override
            public void run() {
                if (webView != null) {
                    webView.destroy();
                    webView = null;
                }
            }
        });
    }
}