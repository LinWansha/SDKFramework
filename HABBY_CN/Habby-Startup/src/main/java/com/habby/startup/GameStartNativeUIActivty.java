package com.habby.startup;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.text.Html;
import android.text.method.LinkMovementMethod;
import android.util.Log;
import android.view.View;
import android.view.Window;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.TextView;
import androidx.annotation.Nullable;

import org.json.JSONException;
import org.json.JSONObject;

import com.habby.startup.utils.JsonUtil;
import com.habby.startup.utils.ReflectionUtil;

public class GameStartNativeUIActivty  extends Activity {

    private String TAG="GameStartNativeUIActivty";
    private Button btnOk;
    private Button btnRefuse;
    private Context _content;


    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        if (IsAppHasLicence()){
            _content = this.getApplicationContext();
            boolean isAgree = SharedPrefsUtil.GetAgreePrivacyResult(_content);
            Log.w(TAG,"--- isAgree=" + isAgree);

            if (!isAgree) {
                DisplayAgreementLayout();
            } else {
                toPermission();
            }
        }
        else {
            toUnity();
        }
    }
    private void  DisplayAgreementLayout(){
        super.requestWindowFeature(Window.FEATURE_NO_TITLE);
        LinearLayout layout = new LinearLayout(getApplicationContext());
        LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MATCH_PARENT, LinearLayout.LayoutParams.MATCH_PARENT);
        layout.setOrientation(LinearLayout.VERTICAL);
        setContentView(layout, params);
        setContentView(R.layout.gamestart);
        btnOk = (Button) findViewById(R.id.ok);

        btnRefuse = (Button) findViewById(R.id.refuse);

        btnOk.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                SharedPrefsUtil.SaveAgreePrivacy(true,_content);
                toPermission();
            }
        });

        btnRefuse.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                finish();
                android.os.Process.killProcess(android.os.Process.myPid());
            }
        });
//读取配置文件中的链接
        String webConfigPath = "assets/SDKConfig/WebConfig.json";
        String gameLicenseUrl = null;
        String privacyUrl = null;
        String childrenProtUrl = null;
        String thirdPartySharingUrl = null;

        try {
            JSONObject jsonObject = JsonUtil.DeserializeFromZip(getPackageResourcePath(),webConfigPath);
            gameLicenseUrl = jsonObject.getString("gameLicenseUrl");
            privacyUrl = jsonObject.getString("privacyUrl");
            childrenProtUrl = jsonObject.getString("childrenProtUrl");
            thirdPartySharingUrl = jsonObject.getString("thirdPartySharingUrl");
        }
        catch (Exception e) {
            Log.e(TAG, "Failed to load JSON file: " + e.getMessage());
        }
//创建带有超链接的文本
        String linksText = String.format("<a href='%s'>《游戏许可及服务协议》</a>、<a href='%s'>《游戏隐私保护指引》</a>、<a href='%s'>《儿童隐私保护指引》</a>和<a href='%s'>《第三方信息共享清单》</a>",
                gameLicenseUrl, privacyUrl, childrenProtUrl, thirdPartySharingUrl);

// 将原始换行符 ("\n") 替换为 HTML 换行符 ("<br>")
        String commonDesWithHtmlLineBreaks =getString(R.string.common_des).replace("\n", "<br>");
//获取无链接的字符串并插入带有超链接的文本

        String commonDes = String.format(commonDesWithHtmlLineBreaks, linksText);

//将含有超链接的文本设置到TextView上
        TextView textView = findViewById(R.id.textviewDes);
        textView.setText(Html.fromHtml(commonDes));
        textView.setMovementMethod(LinkMovementMethod.getInstance());

        Log.i(TAG,textView.toString());
    }
    private boolean IsAppHasLicence() {
        String apkPath = getPackageResourcePath();
        String jsonEntryPath = "assets/SDKConfig/App.json";

        // Deserialize the JSON object from the ZIP file (APK)
        JSONObject jsonObject = JsonUtil.DeserializeFromZip(apkPath, jsonEntryPath);

        try {
            return jsonObject.getBoolean("hasLicense");
        }
        catch (JSONException e) {
            e.printStackTrace();
            return false;
        }
    }

    @Override
    protected void onDestroy()
    {
        super.onDestroy();
        Log.w(TAG,"### destory GameStartNativeUIActivity ");

    }

    private void toPermission()
    {
        Intent intent = new Intent();
        intent.setClass( this, PermissionQuestNativeActivity.class);
        startActivity( intent );
        this.finish();
    }

    private void toUnity()
    {
        Intent intent = new Intent();
        intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP |Intent.FLAG_ACTIVITY_SINGLE_TOP);
        intent.setClass( this, ReflectionUtil.GetUnityPlayerActivity());
        startActivity( intent );
        finish();
    }
}
