namespace Habby.CNUser
{
    [System.Serializable]
    public struct ClientData
    {
        public string deviceId;

        public string appVersion;
        public string osVersion;
        public string appLanguage;
        public string systemLanguage;
        public string appBundle;
        public string deviceModel;
        public string advertisementId;
        public string tgaDistinctId;
        public int channelId;
        public int packageId;
        public int os; //系统 1：ios 2：android
        public string appLocalVersion;//本地app版本
        public string countryCode;
#if UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_IPHONE
        public string teamId;
        public string bundleId;
#endif

    }
}