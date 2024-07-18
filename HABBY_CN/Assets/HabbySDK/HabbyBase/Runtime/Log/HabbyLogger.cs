using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using Habby.Base;
using Habby.Command;


namespace Habby.Log
{
   public enum LogLevel
    {
        Debug = 1,
        Warning,
        Error
    }
    
    public static class HabbyLogger
    // public static class HabbyLogger:UnitySingletonBase<HabbyLogger>
    {
        
        private static LogLevel mLogLevel = LogLevel.Debug;
        private static ILog mLog;
        static HabbyLogger()
        {
            mLog = new UnityLogger();
        }
        
        [Conditional("ENABLE_DEBUG")]
        public static void SetLogger(ILog logger)
        {
            mLog = logger;
        }
        
        public static void SetLogLevel(LogLevel level)
        {
            mLogLevel = level;
        }
        
        //check log level
        public static bool CheckLogLevel(LogLevel level)
        {
            return mLogLevel <= level;
        }
        

        public static void Debug(string msg)
        {
            if (!CheckLogLevel(LogLevel.Debug))
            {
                return;
            }
            mLog.Debug(msg);
        }
        
        public static void Log(string msg,string tag)
        {
            if (!CheckLogLevel(LogLevel.Debug))
            {
                return;
            }
            mLog.Log(msg,tag);
        }



        public static void Warning(string msg)
        {
            if (!CheckLogLevel(LogLevel.Warning))
            {
                return;
            }

            mLog.Warning(msg);
        }

        public static void Error(string msg)
        {
            StackTrace st = new StackTrace(1, true);
            mLog.Error($"{msg}\n{st}");
        }

        public static void Error(Exception e)
        {
            string str = e.ToString();
            mLog.Error(str);
        }

      

        public static void Warning(string message, params object[] args)
        {
            if (!CheckLogLevel(LogLevel.Warning))
            {
                return;
            }
            mLog.Warning(string.Format(message, args));
        }

  

        public static void Debug(string message, params object[] args)
        {
            if (!CheckLogLevel(LogLevel.Debug))
            {
                return;
            }
            mLog.Debug(string.Format(message, args));

        }

        public static void Error(string message, params object[] args)
        {
            StackTrace st = new StackTrace(1, true);
            string s = string.Format(message, args) + '\n' + st;
            mLog.Error(s);
        }
        
        public static void Exception(Exception exception)
        {
            mLog.Exception(exception);
        }
        
        // public static void Console(string message)
        // {
        // }
        //
        // public static void Console(string message, params object[] args)
        // {
        // }
    }
}