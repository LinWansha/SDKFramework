using System;
using Newtonsoft.Json;
using SDKFramework.Message;

namespace SDKFramework.Analytics
{
    
    public interface CloudImpl
    {
        void GetCloudConfig(Action<string,string,int> onComplete);
    }
    public class CloudData
    {
        public bool IsMaskOpen = true;
        
        public bool IsQQRootOpen = true;
        
        public bool IsWxRootOpen = false;
        
        public bool IsQQGroupOpen = true;
        
        public bool IsPrivacyAgree = false;

        public string QQGroupKey = "default";

        #region official pay
        public bool IsWxPayEnable = true;
        public bool IsAliPayEnable = true;
        #endregion

        #region reyun and ranger
        public bool IsDebugEnable = false;
        #endregion

    }
    
    public partial class AnalyticsModule
    {
        private CloudData _cloudData;

        public CloudData CloudData => _cloudData??new CloudData();

        private CloudImpl _cloudImpl;

        private bool CloudInitialized = false;

        public void InitializeCloud(CloudImpl cloud)
        {
            if (CloudInitialized) return;
            CloudInitialized = true;
            _cloudImpl = cloud;
            PullCloudData();
        }

        public void PullCloudData()
        {
            if (!CloudInitialized) return;
            _cloudImpl.GetCloudConfig(((responseStr, msg, code) =>
            {
                Log.Info($"[AnalyticsModule] GetCloudConfig=> code: {code} ,msg: {msg}");

                if (code == 0)
                {
                    try
                    {
                        _cloudData = JsonConvert.DeserializeObject<CloudData>(responseStr);
                        Log.Info($"" +
                         $"\n IsMaskOpen:{_cloudData.IsMaskOpen}" +
                         $"\n IsQQRootOpen:{_cloudData.IsQQRootOpen}" +
                         $"\n IsWxRootOpen:{_cloudData.IsWxRootOpen}" +
                         $"\n IsPrivacyAgree:{_cloudData.IsPrivacyAgree}" +
                         $"\n IsQQGroupOpen:{_cloudData.IsQQGroupOpen}" +
                         $"\n QQGroupKey:{_cloudData.QQGroupKey}");
                        
                        HabbyFramework.Message.Post(new MsgType.RefreshPrivacyToggle(){isOn = _cloudData.IsPrivacyAgree});
                    }
                    catch (Exception e)
                    {
                        #if DEBUG_MODEL
                        Log.Error("CloudData Parse Fail,msg: " + msg);
                        #else
                        Log.Warn("CloudData Parse Error: " + e.Message);
                        #endif
                        _cloudData = new CloudData();
                    }
                    
                }
                else
                    Log.Warn($"云控拉取失败 code: {code} ,msg: {msg}");
            }));
        }
    }
}