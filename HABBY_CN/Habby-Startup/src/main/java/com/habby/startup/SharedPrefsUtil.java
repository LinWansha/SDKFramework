package com.habby.startup;
import android.content.Context;
import android.content.SharedPreferences;


public class SharedPrefsUtil {
    public final static String SETTING = "sdkSetting";
    public final static String PrivacyKey = "agreePrivacy";
    public static boolean _isAgreePrivacy = false;
    public final static String requestPermission = "requestPermission";
    public final static String sdkRequestPermission = "sdkRequestPermission";
    public static void SaveAgreePrivacy(boolean agree,Context context)
    {
        _isAgreePrivacy = agree;
        putValueContent(PrivacyKey,agree,context);
    }

    public static boolean GetAgreePrivacyResult(Context context)
    {
        if (!_isAgreePrivacy)
        {
            _isAgreePrivacy = getValue(PrivacyKey,false,context);
        }
        return _isAgreePrivacy;
    }

    public static void putValueContent(String key, boolean value,Context context) {
        SharedPreferences.Editor sp =  context.getSharedPreferences(SETTING, Context.MODE_PRIVATE).edit();
        sp.putBoolean(key, value);
        sp.commit();
    }

    public static boolean getValue(String key, boolean defValue, Context context) {
        SharedPreferences sp =  context.getSharedPreferences(SETTING, Context.MODE_PRIVATE);
        boolean value = sp.getBoolean(key, defValue);
        return value;
    }

    public static void putValue( String key, long value, Context context) {
        SharedPreferences.Editor sp =  context.getSharedPreferences(SETTING, Context.MODE_PRIVATE).edit();
        sp.putLong(key, value);
        sp.commit();
    }
}

