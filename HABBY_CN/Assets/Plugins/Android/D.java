package com.habby.sdk;

import android.content.Context;
import android.content.res.AssetManager;
import java.io.IOException;
import java.io.InputStream;

public class D {

    private Context context;

    public D(Context context) {
        this.context = context;
    }

    public boolean checkLogSwitch() {
        AssetManager assetManager = context.getAssets();
        InputStream inputStream = null;

        try {
            inputStream = assetManager.open("SDKConfig/logSwitch");
            if (inputStream != null) {
                return true;
            }
        } 
        catch (IOException e) {
            return false;
        } 
        finally {
            if (inputStream != null) {
                try {
                    inputStream.close();
                } 
                catch (IOException e) {
                    e.printStackTrace();
                }
            }
        }
        return false;
    }
}