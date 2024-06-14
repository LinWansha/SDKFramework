using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Aliyun.OSS;
using System;
using System.IO;
using System.Threading;
//using Aliyun.OSS.Common;

namespace SDKFramework.Utils.LogPro
{
    public class HabbyLogUpload
    {
        private const string ENDPOINT = "oss-cn-beijing.aliyuncs.com";
        private const string ACCESS_KEY_ID = "LTAI4FrfmSxosRYaJqzuUKW8";
        private const string ACCESS_KEY_SECRET = "eZvpv01297an8xdtLilvMXGn4XxSgP";
        private const string BUCKET_NAME = "habby-test-log";

        struct UserMetadata
        {
            public string productName;
            public string platformName;
            public string version;
            public string userId;
        }
        private static UserMetadata currentUserMetadata = new UserMetadata
        {
            productName = Application.productName,
            platformName = Application.platform.ToString(),
            version = Application.version,
            userId = "unknownuser"
        };

        //private static OssClient client = new OssClient(ENDPOINT, ACCESS_KEY_ID, ACCESS_KEY_SECRET);
        private static AutoResetEvent _event = new AutoResetEvent(false);
        private static Queue<string> existLogFilePathes;
        public static bool IsUploading { get; private set; } = false;
        public static bool EnableUpload = false;
        public static bool DeleteLocalAfterUpload = false;

        public static void SetUserId(string userId)
        {
            currentUserMetadata.userId = userId;
        }
        public static string CreateLogFileName()
        {
            DateTime now = DateTime.Now;
            return string.Format("{0}_{1}_{2}_{3}_{4}-{5:D2}-{6:D2}-{7:D2}-{8:D2}-{9:D2}",
                currentUserMetadata.productName, currentUserMetadata.platformName, currentUserMetadata.version, currentUserMetadata.userId, now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
        }

        public static void GetExistLogFiles(string logFolderPath)
        {
            string[] pathes;
            if (Directory.Exists(logFolderPath))
                pathes = Directory.GetFiles(logFolderPath, "*.txt");
            else
                pathes = null;
            existLogFilePathes = new Queue<string>();
            if (null != pathes)
                foreach (var path in pathes)
                {
                    existLogFilePathes.Enqueue(path);
                }
        }

        public static void UploadLogFiles()
        {
            if (!EnableUpload)
                return;
            if (null != existLogFilePathes && 0 != existLogFilePathes.Count)
            {
                if (!IsUploading)
                {
                    IsUploading = true;
                    Debug.Log("---------Log files upload start---------");
                }
                string path = existLogFilePathes.Dequeue();
                string[] part = path.Split('/');
                var fileName = string.Format("{0}/{1}/{2}", currentUserMetadata.productName, currentUserMetadata.platformName, part[part.Length - 1]);
                AsyncUploadFile(fileName, path);
            }
            else
            {
                IsUploading = false;
                Debug.Log("----------Log files upload end----------");
            }
        }

        struct AsyncStateResult
        {
            public FileStream fileStream;
            public string message;
        }
        private static void AsyncUploadFile(string objectName, string localFilePath)
        {
            try
            {
                using (var fs = File.Open(localFilePath, FileMode.Open))
                {
                    //var metadata = new ObjectMetadata();
                    // 增加自定义元信息。
                    //metadata.UserMetadata.Add("productName", currentUserMetadata.productName);
                    //metadata.UserMetadata.Add("platformName", currentUserMetadata.platformName);
                    //metadata.UserMetadata.Add("version", currentUserMetadata.version);
                    //metadata.UserMetadata.Add("userId", currentUserMetadata.userId);
                    //metadata.CacheControl = "No-Cache";
                    //metadata.ContentType = "text/html";
                    //metadata.ContentEncoding = "UTF-8";
                    AsyncStateResult asr = new AsyncStateResult { fileStream = fs, message = objectName };

                    // 异步上传。
                    //client.BeginPutObject(BUCKET_NAME, objectName, fs, metadata, UploadObjectCallback, asr);

                    _event.WaitOne();
                }
            }
            // catch (OssException ex)
            // {
            //     Debug.LogErrorFormat("Upload {0} failed with error code: {1}; Error info: {2}. \nRequestID:{3}\tHostID:{4}",
            //         objectName, ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
            //     UploadLogFiles();
            // }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("Upload {0} failed with error info: {1}", objectName, ex.Message);
                UploadLogFiles();
            }
        }
        private static void UploadObjectCallback(IAsyncResult ar)
        {
            try
            {
                //client.EndPutObject(ar);
                AsyncStateResult asr = (AsyncStateResult)ar.AsyncState;
                if (null != asr.fileStream)
                {
                    asr.fileStream.Close();
                    if (DeleteLocalAfterUpload)
                        File.Delete(asr.fileStream.Name);
                }
                Debug.LogFormat("Upload log file succeeded. FileName:{0}", asr.message);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
            finally
            {
                _event.Set();
                UploadLogFiles();
            }
        }
    }
}
