namespace SDKFramework.Config
{
    [System.Serializable]
    public struct AppConfig
    {
        public bool IsNull { get; private set; }
        public static AppConfig Null { get; } = new AppConfig() { IsNull = true };
        
        public bool hasLicense;
        public string gameName;
        public ApplicableRange applicableRange;
        public string details;

        public enum ApplicableRange
        {
            Range8 = 8,
            Range12 = 12,
            Range16 = 16,
        }
    }
}