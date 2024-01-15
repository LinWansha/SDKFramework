using System;
using System.Diagnostics;
using Habby.Log;
using Color= UnityEngine.Color;
using Debug = UnityEngine.Debug;

/// <summary>
/// 编辑器模式下要开启 ENABLE_DEBUG 才能正常使用，
/// 这次封装目的是为了，不需要在调试完之后删掉之前的日志，
/// 并且后续如果出问题可以直接开启 ENABLE_DEBUG 看日志
/// 有其余扩展需求后续加入
/// </summary>
public class HLog
{
    [Conditional("ENABLE_DEBUG")]
    public static void Log(string context ,Color color)
    {
        Debug.Log(string.Format("<color="+ ToHexColor(color) + ">{0}</color>", context));
    }
        
    [Conditional("ENABLE_DEBUG")]
    public static void LogException(Exception exception)
    {
        Debug.LogException(exception);
    }

    [Conditional("ENABLE_DEBUG")]
    public static void LogFormat(string format, params object[] args)
    {
        HabbyLogOutput.Init();
        UnityEngine.Debug.LogFormat(format, args);
    }

    [Conditional("ENABLE_DEBUG")]
    public static void LogErrorFormat(string format, params object[] args)
    {
        HabbyLogOutput.Init();
        UnityEngine.Debug.LogErrorFormat(format, args);
    }

    [Conditional("ENABLE_DEBUG")]
    public static void Log(object message)
    {
        HabbyLogOutput.Init();
        UnityEngine.Debug.Log(message);
    }

    [Conditional("ENABLE_DEBUG")]
    public static void LogWarning(object message)
    {
        HabbyLogOutput.Init();
        UnityEngine.Debug.LogWarning(message);
    }

    [Conditional("ENABLE_DEBUG")]
    public static void LogError(object message)
    {
        HabbyLogOutput.Init();
        UnityEngine.Debug.LogError(message);
    }

    [Conditional("ENABLE_DEBUG")]
    public static void Assert(bool condition, string info)
    {
        HabbyLogOutput.Init();
        if (condition) return;
        UnityEngine.Debug.LogError(info);
    }
    
    [Conditional("ENABLE_DEBUG")]
    public static void LogWarnFormat(string format, params object[] args) {
        UnityEngine.Debug.LogWarningFormat(format, args);
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