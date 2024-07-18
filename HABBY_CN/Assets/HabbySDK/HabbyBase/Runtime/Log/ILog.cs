using System;

namespace Habby.Log
{
    public interface ILog
    {
        void Warning(string message);
        void Debug(string message);
        void Error(string message);
        void Exception(Exception exception);
        void Warning(string message, params object[] args);
        void Debug(string message, params object[] args);
        void Error(string message, params object[] args);
        void Log(string message,string tag);
    }
}