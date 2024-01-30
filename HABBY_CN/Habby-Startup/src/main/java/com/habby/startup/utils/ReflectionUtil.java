package com.habby.startup.utils;

import android.util.Log;

import java.lang.reflect.Constructor;

public class ReflectionUtil {

    public static Class GetUnityPlayerActivity(){
        try {
            // 使用全限定类名 (包含包名) 获取 UnityPlayerActivity 的 Class 对象
            Class<?> unityPlayerActivityClass = Class.forName("com.unity3d.player.UnityPlayerActivity");
            Log.e("ReflectionUtil","通过反射创建UnityPlayerActivity成功");
            // 使用反射创建 UnityPlayerActivity 实例
            //Constructor<?> constructor = unityPlayerActivityClass.getConstructor();
            //Object unityPlayerActivityInstance = constructor.newInstance();
            return unityPlayerActivityClass;

        } catch (Exception e) {
            Log.e("ReflectionUtil","通过反射创建UnityPlayerActivity失败");
            e.printStackTrace();
            return null;
        }
    }
}
