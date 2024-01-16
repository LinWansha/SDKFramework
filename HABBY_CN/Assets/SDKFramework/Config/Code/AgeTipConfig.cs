namespace SDKFramework.Config
{
    public struct AgeTipConfig
    {
        public bool IsNull { get; private set; }
        public static AgeTipConfig Null { get; } = new AgeTipConfig() { IsNull = true };
        
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