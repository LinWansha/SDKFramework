using System;
using Newtonsoft.Json;

namespace SDKFramework.Analytics
{
    
    public interface CloudImpl
    {
        void GetCloudConfig(Action<string,string,int> onComplete);
    }
    public class CloudData
    {
        public bool flowerMaskOpen = true;
        
        public bool IsQQRootOpen = true;
        
        public bool IsWxRootOpen = false;
        
        public bool IsQQGroupOpen = true;
        
        public string QQGroupKey = "default";

        #region official pay
        public bool IsWxPayEnable = true;
        public bool IsAliPayEnable = true;
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
                    
                    Log.Info("FlowerMaskOpen" + _cloudData.flowerMaskOpen);
                    Log.Info("IsQQRootOpen" + _cloudData.IsQQRootOpen);
                    Log.Info("IsWxRootOpen" + _cloudData.IsWxRootOpen);
                    Log.Info("IsQQGroupOpen" + _cloudData.IsQQGroupOpen);
                }
                else
                    Log.Warn($"云控拉取失败 code: {code} ,msg: {msg}");
            }));
        }
    }
}