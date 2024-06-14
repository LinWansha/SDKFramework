using System;
using System.Diagnostics;
using SDKFramework.Utils.LogPro;
using Color = UnityEngine.Color;
using Debug = UnityEngine.Debug;


public class Log
{
    static Log()
    {
        //HabbyLogOutput.Init(/* userid */);
    }

    [Conditional("ENABLE_DEBUG")]
    public static void Info(object message, string tag = null)
    {
        Debug.Log(tag == null ? message : $"{tag} {message}");
    }

    [Conditional("ENABLE_DEBUG")]
    public static void Warn(object message, string tag = null)
    {
        Debug.LogWarning(message);
    }

    [Conditional("ENABLE_DEBUG")]
    public static void Error(object message, string tag = null)
    {
        Debug.LogError(message);
    }

    [Conditional("ENABLE_DEBUG")]
    public static void Assert(bool condition, string message, string tag = null)
    {
        if (condition) return;
        Error(message, tag);
    }

    [Conditional("ENABLE_DEBUG")]
    public static void Exception(Exception exception)
    {
        Debug.LogException(exception);
    }

    internal static string ToHexColor(Color color)
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