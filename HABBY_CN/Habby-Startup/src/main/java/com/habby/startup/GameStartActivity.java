package com.habby.startup;

import android.content.Intent;
import android.net.Uri;
import android.util.Log;
import android.view.Window;
import android.webkit.WebView;
import android.widget.LinearLayout;
import android.app.Activity;
import android.os.Bundle;
import android.view.View;
import android.webkit.WebSettings;
import android.webkit.WebViewClient;
import android.widget.LinearLayout.LayoutParams;



import java.lang.ref.WeakReference;
import java.util.Set;

import androidx.annotation.Nullable;

import com.habby.startup.utils.ReflectionUtil;

public class GameStartActivity extends Activity {

    private class MyWebViewClient extends WebViewClient {

        //弱引用持有HandlerActivity , GC 回收时会被回收掉
        private WeakReference<Activity> weakReference;

        public MyWebViewClient(Activity activity) {
            this.weakReference = new WeakReference(activity);
        }

        @Override
        public boolean shouldOverrideUrlLoading(final WebView view, String url) {
            Log.w("webview","--- shouldOverrideUrlLoading url=" + url);
            if (!(url.startsWith("unityevent"))) {
                return true;
            }

            // 步骤2：根据协议的参数，判断是否是所需要的url
            // 一般根据scheme（协议格式） & authority（协议名）判断（前两个参数）
//            unityevent://userAgreement?attitude=true
            Uri uri = Uri.parse(url);
            // 如果url的协议 = 预先约定的 userAgreement 协议
            // 就解析往下解析参数
            Log.w("webview","--- shouldOverrideUrlLoading getScheme=" + uri.getScheme());
            if (uri.getScheme().equals("unityevent")) {

                // 如果 authority  = 预先约定协议里的 webview，即代表都符合约定的协议
                // 所以拦截url,下面JS开始调用Android需要的方法
                Log.w("webview","--- shouldOverrideUrlLoading getScheme=" + uri.getAuthority());
                if (uri.getAuthority().equals("userAgreement")) {
                    //  步骤3：
                    // 执行JS所需要调用的逻辑
                    System.out.println("js调用了Android的方法");

                    // 可以在协议上带有参数并传递到Android上
                    Set<String> collection = uri.getQueryParameterNames();
                    String result = uri.getQueryParameter("attitude");
                    Log.w("gamestart","---   result=" + result);
                    if(result.equals("true"))
                    {
//                        SDKHubManager.getInstance().setValue(SDKHubDefine.KEY_IS_FIRST_OPEN,true);
                        SharedPrefsUtil.SaveAgreePrivacy(true,this.weakReference.get());
                        toUnity();
                    }
                    else
                    {
                           // 退出
                        this.weakReference.get().finish();
                    }
                }
                return true;
            }
            return super.shouldOverrideUrlLoading(view, url);
        }
    }


    private WebView mWebView;

    private void toUnity()
    {
        Intent intent = new Intent();
        intent.setClass( this, ReflectionUtil.GetUnityPlayerActivity());
        intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP |Intent.FLAG_ACTIVITY_SINGLE_TOP);
        startActivity( intent );
        this.finish();
    }

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        boolean isAgree = false;
        isAgree = SharedPrefsUtil.GetAgreePrivacyResult(this);
        Log.w("Tag","--- isAgree=" + isAgree);
        if(!isAgree)
        {
            super.requestWindowFeature(Window.FEATURE_NO_TITLE);
            LinearLayout layout = new LinearLayout(getApplicationContext());
            LayoutParams params = new LayoutParams(LayoutParams.MATCH_PARENT, LayoutParams.MATCH_PARENT);
            layout.setOrientation(LinearLayout.VERTICAL);
            setContentView(layout, params);

            mWebView = new WebView(getApplicationContext());
            params.weight = 1;
            mWebView.setVisibility(View.VISIBLE);
            layout.addView(mWebView, params);

            WebSettings settings = mWebView.getSettings();
            settings.setJavaScriptEnabled(true);
            settings.setJavaScriptCanOpenWindowsAutomatically(true);
            mWebView.setVerticalScrollbarOverlay(true);
            mWebView.setWebViewClient(new MyWebViewClient(this));

            boolean isDebug = BuildConfig.DEBUG;
            if(isDebug)
            {
                // 启用 WebView 调试模式。
                // 注意：请勿在实际 App 中打开！
                WebView.setWebContentsDebuggingEnabled(true);
                mWebView.loadUrl("https://test-h5-survivorio.lezuan.net/agreement/index.html");
            }
            else{
                mWebView.loadUrl("https://h5-survivorio.lezuan.net/agreement/index.html");
            }
        }
        else
        {
            // 切换到unity
            toUnity();
        }
    }

}
