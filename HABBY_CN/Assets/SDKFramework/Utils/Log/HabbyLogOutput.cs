using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using UnityEngine;

/* Notice Ror IOS
 * Please add
 * "Application supports iTunes file sharing" "Supports opening documents in place" Key
 * to xcode's Custom IOS Target Properties , and set them value to YES.
 * Then you can check Log files in "Files" App (Need IOS 11 Or later)
 */
namespace Habby.Log
{
    public class HabbyLogOutput
    {
        private static HabbyLogOutput _instance;

        private static string _logFloderPath;

        private static string logFloderPath
        {
            get
            {
                if (string.IsNullOrEmpty(_logFloderPath) && string.IsNullOrEmpty(HabbyLogOutputToFile.devicePersistentPath) == false)
                {
                    _logFloderPath = string.Format("{0}/{1}", HabbyLogOutputToFile.devicePersistentPath, HabbyLogOutputToFile.logFloderPath);
                }

                return _logFloderPath;
            }
        }

        [Conditional("ENABLE_DEBUG")]
        public static void Init(bool enableUpload = false, bool deleteLocalAfterUpload = false)
        {
#if !UNITY_EDITOR
            if (null == _instance)
            {
                HabbyLogUpload.EnableUpload = enableUpload;
                HabbyLogUpload.DeleteLocalAfterUpload = deleteLocalAfterUpload;
                HabbyLogUpload.GetExistLogFiles(logFloderPath);
                _instance = new HabbyLogOutput();
                HabbyLogUpload.UploadLogFiles();
            }
#endif
        }
        [Conditional("ENABLE_DEBUG")]
        public static void Init(string userId, bool enableUpload = false, bool deleteLocalAfterUpload = false)
        {
#if !UNITY_EDITOR
            if (null == _instance)
            {
                HabbyLogUpload.SetUserId(userId);
                HabbyLogUpload.EnableUpload = enableUpload;
                HabbyLogUpload.DeleteLocalAfterUpload = deleteLocalAfterUpload;
                HabbyLogUpload.GetExistLogFiles(logFloderPath);
                _instance = new HabbyLogOutput();
                HabbyLogUpload.UploadLogFiles();
            }
#endif
        }

        private int mainThreadID = -1;

        private List<ILogOutput> logOutputList = null;

        private HabbyLogOutput()
        {
            this.mainThreadID = Thread.CurrentThread.ManagedThreadId;

            this.logOutputList = new List<ILogOutput>
            {
                new HabbyLogOutputToFile(),
            };

            Application.logMessageReceived += LogCallback;
            Application.logMessageReceivedThreaded += LogMultiThreadCallback;
            Application.quitting += OnQuit;
        }

        void LogCallback(string logStr, string trackStr, LogType type)
        {
            if (this.mainThreadID == Thread.CurrentThread.ManagedThreadId)
                Output(logStr, trackStr, type);
        }

        void LogMultiThreadCallback(string logStr, string trackStr, LogType type)
        {
            if (this.mainThreadID != Thread.CurrentThread.ManagedThreadId)
                Output(logStr, trackStr, type);
        }

        void Output(string logStr, string trackStr, LogType type)
        {
            HabbyLogData logData = new HabbyLogData
            {
                log = logStr,
                track = trackStr,
                logType = type,
            };
            for (int i = 0; i < this.logOutputList.Count; ++i)
                this.logOutputList[i].Log(logData);
        }
        void OnQuit()
        {
            Application.logMessageReceived -= LogCallback;
            Application.logMessageReceivedThreaded -= LogMultiThreadCallback;
        }
    }

    public class HabbyLogOutputToFile : ILogOutput
    {

        private static string _devicePersistentPath;
        public static string devicePersistentPath
        {
            get
            {
                if (string.IsNullOrEmpty(_devicePersistentPath))
                {
#if UNITY_EDITOR
                    _devicePersistentPath = Application.dataPath + "/../PersistentPath";
#elif UNITY_STANDALONE_WIN
                    _devicePersistentPath = Application.dataPath + "/PersistentPath";
#elif UNITY_STANDALONE_OSX
                    _devicePersistentPath = Application.dataPath + "/PersistentPath";
#else
                    _devicePersistentPath = Application.persistentDataPath;
#endif
                }

                return _devicePersistentPath;
            }
        }

        public static string logFloderPath = "Log";

        private Queue<HabbyLogData> mWritingLogQueue = null;
        private Queue<HabbyLogData> mWaitingLogQueue = null;
        private object mLogLock = null;
        private Thread mFileLogThread = null;
        private bool mIsRunning = false;
        private StreamWriter mLogWriter = null;

        public HabbyLogOutputToFile()
        {
            this.mWritingLogQueue = new Queue<HabbyLogData>();
            this.mWaitingLogQueue = new Queue<HabbyLogData>();
            this.mLogLock = new object();

           // string logName = HabbyLogUpload.CreateLogFileName();
           string logName = "can,t use";
            string logPath = string.Format("{0}/{1}/{2}.txt", devicePersistentPath, logFloderPath, logName);
            if (File.Exists(logPath))
                File.Delete(logPath);
            string logDir = Path.GetDirectoryName(logPath);
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);
            this.mLogWriter = new StreamWriter(logPath);
            this.mLogWriter.AutoFlush = true;
            this.mIsRunning = true;
            this.mFileLogThread = new Thread(new ThreadStart(WriteLog));
            this.mFileLogThread.Start();

            Application.quitting += Close;
        }

        void WriteLog()
        {
            while (this.mIsRunning)
            {
                if (this.mWritingLogQueue.Count == 0)
                {
                    lock (this.mLogLock)
                    {
                        while (this.mWaitingLogQueue.Count == 0)
                            Monitor.Wait(this.mLogLock);
                        Queue<HabbyLogData> tmpQueue = this.mWritingLogQueue;
                        this.mWritingLogQueue = this.mWaitingLogQueue;
                        this.mWaitingLogQueue = tmpQueue;
                    }
                }
                else
                {
                    while (this.mWritingLogQueue.Count > 0)
                    {
                        HabbyLogData log = this.mWritingLogQueue.Dequeue();
                        if (LogType.Error == log.logType || LogType.Exception == log.logType || LogType.Assert == log.logType)
                        {
                            this.mLogWriter.WriteLine("┌──────────────────────────────────ERROR──────────────────────────────────┐");
                            this.mLogWriter.WriteLine("\n" + System.DateTime.Now.ToString() + "\t" + log.log + "\n");
                            this.mLogWriter.WriteLine(log.track);
                            this.mLogWriter.WriteLine("└─────────────────────────────────────────────────────────────────────────┘");
                        }
                        else
                        {
                            this.mLogWriter.WriteLine(System.DateTime.Now.ToString() + "\t" + log.log);
                        }
                    }
                }
            }
        }

        public void Log(HabbyLogData logData)
        {
            lock (this.mLogLock)
            {
                this.mWaitingLogQueue.Enqueue(logData);
                Monitor.Pulse(this.mLogLock);
            }
        }

        public void Close()
        {
            this.mIsRunning = false;
            this.mLogWriter.Close();
            try
            {
                this.mFileLogThread.Abort();
            }
            catch (ThreadInterruptedException)
			{
            }
        }
    }

    public class HabbyLogData
    {
        public string log { get; set; }
        public string track { get; set; }
        public LogType logType { get; set; }
    }

    public interface ILogOutput
    {
        void Log(HabbyLogData logData);
        void Close();
    }
}