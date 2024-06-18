namespace SDKFramework.Config
{
    [System.Serializable]
    public struct WebConfig
    {
        public bool IsNull { get; private set; }
        public static WebConfig Null { get; } = new WebConfig() { IsNull = true };
        
        /// <summary>
        /// 游戏许可及服务协议
        /// </summary>
        public string gameLicenseUrl;
        
        /// <summary>
        /// 游戏隐私保护指引
        /// </summary>
        public string gamePrivacyUrl;
        
        /// <summary>
        /// 儿童隐私保护指引
        /// </summary>
        public string childrenPrivacyUrl;
        
        /// <summary>
        /// 第三方信息共享清单
        /// </summary>
        public string thirdPartySharingUrl;
        
        /// <summary>
        /// 个人信息清单
        /// </summary>
        public string personInfoListUrl;
    }
}