using System;
using System.Diagnostics;
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
    
    public static void Info(object message,Color color)
    {
        Info($"<color={color.ToHexColor()}>{message}</color>");
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

}