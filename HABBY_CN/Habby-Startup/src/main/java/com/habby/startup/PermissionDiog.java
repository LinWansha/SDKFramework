package com.habby.startup;

import android.app.Dialog;
import android.content.Context;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.ListView;
import java.util.ArrayList;
import java.util.List;

public class PermissionDiog extends Dialog {
    private Button btnOk;
    private OnPermissionClose callback;
    public PermissionDiog(Context context,OnPermissionClose close) {
        super(context, R.style.ActionSheetDialogStyle);
        callback = close;
    }

    @Override
    public void onBackPressed()
    {
        return;
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.permision);
        btnOk = (Button) findViewById(R.id.entrue);
        Context myContext = getContext();
        btnOk.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
//                Intent intent = new Intent();
//                intent.setClass( myContext, Activity2.class);
//                getContext().startActivity( intent );
//                getOwnerActivity().finish();
                dismiss();
                callback.OnPermissionClose();
                callback = null;
            }
        });

        List<String> data = new ArrayList<String>();
        data.add("1、获取IMEI：此权限主要用于游戏内广告追踪，向您推荐个性化广告；");
        data.add("2、安装应用权限：点击广告可以直接安装您想要的应用。");
        data.add("3、读取电话状态：手机一键登录验证设备合法性。");
//        data.add("3、安装应用权限：点击广告可以直接安装您想要的应用。");
//        data.add("4、安装应用权限：点击广告可以直接安装您想要的应用。");
//        data.add("5、安装应用权限：点击广告可以直接安装您想要的应用。");

        // 通过ArrayAdapter将数组中的数据传给ListView
        ListAdapter adapter = new ListAdapter(getContext(), R.layout.item,data);
        ListView listView = findViewById(R.id.permissionList);
        // setAdapter()方法可以将构建好的适配器对象传递进去
        listView.setAdapter(adapter);
    }
}
