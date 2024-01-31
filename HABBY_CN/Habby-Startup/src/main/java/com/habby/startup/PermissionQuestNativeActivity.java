package com.habby.startup;

import android.Manifest;
import android.annotation.SuppressLint;
import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.os.Build;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.ListView;

import java.util.ArrayList;
import java.util.List;

import androidx.annotation.Nullable;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;
import androidx.core.content.PermissionChecker;

import com.habby.startup.utils.ReflectionUtil;

public class PermissionQuestNativeActivity extends Activity implements OnPermissionClose {

    private static final int CODE = 20220825;
    private PermissionDiog permissionDiog;
    private void toUnity()
    {
        Intent intent = new Intent();
        intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP |Intent.FLAG_ACTIVITY_SINGLE_TOP);
        intent.setClass( this, ReflectionUtil.GetUnityPlayerActivity());
        startActivity( intent );
        finish();
    }
    @Override
    public void onBackPressed()
    {
        return;
    }

    @Override
    public void OnPermissionClose() {
        permissionDiog = null;
        checkAndRequestPermission();
    }

    private Button btnOk;

    @SuppressLint("ResourceType")
    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
       boolean request = SharedPrefsUtil.getValue(SharedPrefsUtil.requestPermission,false,this.getApplicationContext());
       if (request)
       {
           toUnity();
           return;
       }
        //SDKHubManager.getInstance().addActivity(this);

        setContentView(R.layout.permision);

        overridePendingTransition(0, 0);
        btnOk = (Button) findViewById(R.id.entrue);
        btnOk.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                checkAndRequestPermission();
            }
        });

        List<String> data = new ArrayList<String>();
        data.add("1、获取设备信息：包括IMEI、IMSI、android id、device id等设备ID，此权限主要用于游戏内广告追踪；用于上报错误日志，便于我们分析解决游戏内出现的严重问题；用于登录验证设备合法性；用于数据分析。");
        data.add("2、获取定位：此权限主要用于游戏内广告追踪、向您推荐个性化广告、用于广告投放。");
        data.add("3、安装应用权限：点击广告可以直接安装您想要的应用。");
        data.add("4、获取应用列表信息：用于游戏安全及反外挂。");
        data.add("5、获取安装应用列表：用于游戏安全及反外挂、保证广告正确投放。");
        data.add("6、获取存储：为了实现广告缓存和使用、游戏数据的保存与读取。");
        data.add("7、读写相册权限：分享游戏功能保存和读取图片。");
        data.add("8、读取通话状态和移动网络信息：手机一键登录和网络类型判断");
        data.add("9、网络访问：游戏通信，广告拉取，sdk功能性通信");
        data.add("10、执行SHELL命令 ：游戏，广告，bugly，一键登录等功能执行系统服务");
        data.add("11、读写手机外置存储信息 ：腾讯开放平台SDK保存和读取游戏信息，保存游戏截图等");
        data.add("12、获取WiFi_SSID ：广告，移动分析sdk获取网络状态。");
        data.add("13、自启动或关联启动 ：广告sdk权限，当点击广告时触发启动或者关联启动。");

        // 通过ArrayAdapter将数组中的数据传给ListView
        ListAdapter adapter = new ListAdapter(this, R.layout.item,data);
        ListView listView = findViewById(R.id.permissionList);
        // setAdapter()方法可以将构建好的适配器对象传递进去
        listView.setAdapter(adapter);

//        Log.w("Tag","### create PermiassionQuestActivity ");
//        super.requestWindowFeature(Window.FEATURE_NO_TITLE);
//        LinearLayout layout = new LinearLayout(this);
//        LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MATCH_PARENT, LinearLayout.LayoutParams.MATCH_PARENT);
//        layout.setOrientation(LinearLayout.VERTICAL);
//        setContentView(layout, params);
//
////        setContentView(R.style.ActionSheetDialogStyle, R.style.ActionSheetDialogStyle);
//        permissionDiog = new PermissionDiog(this,this);
//        permissionDiog.show();

    }


    //// for permission
    /**
     * ----------非常重要----------
     * Android6.0以上的权限适配简单示例：
     * 如果targetSDKVersion >= 23，那么必须要申请到所需要的权限，再调用广点通SDK，否则广点通SDK不会工作。
     */
    private int total;
    private int count;

    protected void checkAndRequestPermission() {
        List<String> lackedPermission = new ArrayList<String>();
        List<String> necessaryPermissions = getNessaryPermissiions();
        Context context = this;

        for (String necessaryPermission : necessaryPermissions) {
////            当编译targetSDKVersion < 23时使用
//            PermissionChecker.checkSelfPermission
//
////            同样targetSDKVersion >= 23时PermissionChecker.checkSelfPermission也会无效，
//
//            ContextCompat.checkSelfPermission
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {

                if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O)
                {
                    boolean haveInstallPermission = getPackageManager().canRequestPackageInstalls();
                    Log.d("Permission: ", "--- haveInstallPermission:" +haveInstallPermission);
                }

                if (ContextCompat.checkSelfPermission(context, necessaryPermission) != PackageManager.PERMISSION_GRANTED) {
                    Log.d("checkAndReqPermission: ", necessaryPermission + " not GRANTED");
                    lackedPermission.add(necessaryPermission);
                }
                else
                {
                    Log.d("checkAndReqPermission: ", necessaryPermission + " GRANTED");
                }
            }
            else
            {
                if (PermissionChecker.checkSelfPermission(context, necessaryPermission) != PermissionChecker.PERMISSION_GRANTED) {
                    Log.d("checkAndReqPermission: ", necessaryPermission + " not GRANTED");
                    lackedPermission.add(necessaryPermission);
                }
                else
                {
                    Log.d("checkAndReqPermission: ", necessaryPermission + " GRANTED");
                }
            }

//            LogUtil.debug("--- checkAndRequestPermission: ", necessaryPermission);
//            if (ActivityCompat.checkSelfPermission(context, necessaryPermission) != PackageManager.PERMISSION_GRANTED) {
//                LogUtil.debug("checkAndRequestPermission: ", necessaryPermission + " not GRANTED");
//                lackedPermission.add(necessaryPermission);
//            }
//            else
//            {
//                LogUtil.debug("checkAndRequestPermission: ", necessaryPermission + " GRANTED");
//            }
        }
        SharedPrefsUtil.putValueContent(SharedPrefsUtil.requestPermission,true,this.getApplicationContext());
        // 权限都已经有了，那么直接调用SDK
        if (lackedPermission.size() == 0) {
            Log.d("PermissionsPlugin","--- all nesssary permissiion ok");
            toUnity();
        } else {
            total = lackedPermission.size();
            count = 0;
            // 请求所缺少的权限，在onRequestPermissionsResult中再看是否获得权限，如果获得权限就可以调用SDK，否则不要调用SDK。
            String[] requestPermissionNames = new String[lackedPermission.size()];
            lackedPermission.toArray(requestPermissionNames);

//            LogUtil.debug("--- requestPermissionNames =" + requestPermissionNames[0] + "," + requestPermissionNames[1] );
//            requestPermissions( requestPermissionNames, SDKHubDefine.PERMISSION_CODE);
            ActivityCompat.requestPermissions(this, requestPermissionNames, CODE);
//            permissionCheck = this.checkSelfPermission(Manifest.permission.ACCESS_FINE_LOCATION);       //允许一个程序访问精良位置(如GPS)
//            permissionCheck += this.checkSelfPermission(Manifest.permission.ACCESS_COARSE_LOCATION);    //允许一个程序访问CellID或WiFi热点来获取粗略的位置
//            permissionCheck += this.checkSelfPermission(Manifest.permission.CAMERA);
        }
    }


    protected boolean HasRequestPermission() {
        List<String> lackedPermission = new ArrayList<String>();
        List<String> necessaryPermissions = getNessaryPermissiions();
        Context context = this;

        for (String necessaryPermission : necessaryPermissions) {
////            当编译targetSDKVersion < 23时使用
//            PermissionChecker.checkSelfPermission
//
////            同样targetSDKVersion >= 23时PermissionChecker.checkSelfPermission也会无效，
//
//            ContextCompat.checkSelfPermission
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {

                if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O)
                {
                    boolean haveInstallPermission = getPackageManager().canRequestPackageInstalls();
                    Log.d("Permission: ", "--- haveInstallPermission:" +haveInstallPermission);
                }

                if (ContextCompat.checkSelfPermission(context, necessaryPermission) != PackageManager.PERMISSION_GRANTED) {
                    Log.d("checkAndReqPermission: ", necessaryPermission + " not GRANTED");
                    lackedPermission.add(necessaryPermission);
                }
                else
                {
                    Log.d("checkAndReqPermission: ", necessaryPermission + " GRANTED");
                }
            }
            else
            {
                if (PermissionChecker.checkSelfPermission(context, necessaryPermission) != PermissionChecker.PERMISSION_GRANTED) {
                    Log.d("checkAndReqPermission: ", necessaryPermission + " not GRANTED");
                    lackedPermission.add(necessaryPermission);
                }
                else
                {
                    Log.d("checkAndReqPermission: ", necessaryPermission + " GRANTED");
                }
            }

//            LogUtil.debug("--- checkAndRequestPermission: ", necessaryPermission);
//            if (ActivityCompat.checkSelfPermission(context, necessaryPermission) != PackageManager.PERMISSION_GRANTED) {
//                LogUtil.debug("checkAndRequestPermission: ", necessaryPermission + " not GRANTED");
//                lackedPermission.add(necessaryPermission);
//            }
//            else
//            {
//                LogUtil.debug("checkAndRequestPermission: ", necessaryPermission + " GRANTED");
//            }
        }
        Log.d("lackedPermission", "HasRequestPermission: "+lackedPermission.size());
        // 权限都已经有了，那么直接调用SDK
        if (lackedPermission.size() == 0) {
           return false;
        } else {
           return true;
        }
    }


    private ArrayList<String> getNessaryPermissiions()
    {
        ArrayList<String> list = new ArrayList<String>();
        list.add(Manifest.permission.REQUEST_INSTALL_PACKAGES);
        list.add(Manifest.permission.READ_PHONE_STATE);
        list.add(Manifest.permission.QUERY_ALL_PACKAGES);
        list.add("com.android.permission.GET_INSTALLED_APPS");
        return list;
    }

    @Override
    protected void onDestroy()
    {
        super.onDestroy();
        Log.w("Tag","### destory PermiassionQuestActivity ");
        //SDKHubManager.getInstance().removeActivity(this);
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        for (int i = 0; i < grantResults.length; i++) {
            Log.d("onReqPermissionsResult","--- permission="+ permissions[i] + ",result=" + grantResults[i]);
        }
        Context applicationContext = this.getApplicationContext();
        boolean granted = ContextCompat.checkSelfPermission(applicationContext, Manifest.permission.READ_PHONE_STATE) == PackageManager.PERMISSION_GRANTED;
        if(!granted)
        {
            Log.d("onReqPermissionsResult","--- set READ_PHONE_STATE=false");
            SharedPrefsUtil.putValue(Manifest.permission.READ_PHONE_STATE,System.currentTimeMillis(),applicationContext);
        }

        if(requestCode == CODE)
        {
            toUnity();
        }
    }
}
