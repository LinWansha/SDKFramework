using System;
using System.Diagnostics;
using SDKFramework.Utils.Log;
using Color = UnityEngine.Color;
using Debug = UnityEngine.Debug;

/// <summary>
/// 编辑器模式下要开启 ENABLE_DEBUG 才能正常使用，
/// 这次封装目的是为了，不需要在调试完之后删掉之前的日志，
/// 并且后续如果出问题可以直接开启 ENABLE_DEBUG 看日志
/// 有其余扩展需求后续加入
/// </summary>
public class HLogger
{
    [Conditional("ENABLE_DEBUG")]
    public static void Log(string context, Color color)
    {
#if USE_NATIVE_LOG
        Console.WriteLine("<color=" + ToHexColor(color) + ">{0}</color>", context);
#else
        Debug.Log(string.Format("<color=" + ToHexColor(color) + ">{0}</color>", context));
#endif
    }

    [Conditional("ENABLE_DEBUG")]
    public static void LogFormat(string format, params object[] args)
    {
        HabbyLogOutput.Init();
#if USE_NATIVE_LOG
        Console.WriteLine(format,args);
#else
        Debug.LogFormat(format, args);
#endif
    }

    [Conditional("ENABLE_DEBUG")]
    public static void LogErrorFormat(string format, params object[] args)
    {
        HabbyLogOutput.Init();
#if USE_NATIVE_LOG
        Console.WriteLine(format,args);
#else
        Debug.LogErrorFormat(format, args);
#endif
    }

    [Conditional("ENABLE_DEBUG")]
    public static void Log(object message)
    {
        HabbyLogOutput.Init();
#if USE_NATIVE_LOG
        Console.WriteLine(message);
#else
        Debug.Log(message);
#endif
    }

    [Conditional("ENABLE_DEBUG")]
    public static void LogWarning(object message)
    {
        HabbyLogOutput.Init();
#if USE_NATIVE_LOG
        Console.WriteLine(message);
#else
        Debug.LogWarning(message);
#endif
    }

    [Conditional("ENABLE_DEBUG")]
    public static void LogError(object message)
    {
        HabbyLogOutput.Init();
#if USE_NATIVE_LOG
        Console.WriteLine(message);
#else
        Debug.LogError(message);
#endif
    }



    [Conditional("ENABLE_DEBUG")]
    public static void LogWarnFormat(string format, params object[] args)
    {
#if USE_NATIVE_LOG
        Console.WriteLine(format, args);
#else
        Debug.LogWarningFormat(format, args);
#endif
    }
    
    [Conditional("ENABLE_DEBUG")]
    public static void Assert(bool condition, string info)
    {
        HabbyLogOutput.Init();
        if (condition) return;
        Debug.LogError(info);
    }
    
    [Conditional("ENABLE_DEBUG")]
    public static void LogException(Exception exception)
    {
        Debug.LogException(exception);
    }

    private static string ToHexColor(Color color)
    {
        if (color == Color.white)
            return "#000000";
        string R = Convert.ToString((int)color.r * 255, 16);
        if (R == "0")
            R = "00";
        string G = Convert.ToString((int)color.g * 255, 16);
        if (G == "0")
            G = "00";
        string B = Convert.ToString((int)color.b * 255, 16);
        if (B == "0")
            B = "00";
        string HexColor = "#" + R + G + B;
        return HexColor.ToUpper();
    }
}