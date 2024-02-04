namespace SDKFramework.Config
{
    [System.Serializable]
    public struct WebConfig
    {
        public bool IsNull { get; private set; }
        public static WebConfig Null { get; } = new WebConfig() { IsNull = true };
        
        public string gameLicenseUrl;
        public string privacyUrl;
        public string childrenProtUrl;
        public string thirdPartySharingUrl;
    }
}