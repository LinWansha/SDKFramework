using System;
using System.Collections.Generic;
using System.Globalization;
using SDKFramework.Account.DataSrc;
using UnityEngine;

namespace SDKFramework.Account.Net
{
    public class HabbyUserClient:SDKFramework.Utils.MonoSingleton<HabbyUserClient>
    {
        private const string TRACK_DISABLED = "TrackingDisabled";

        private const string PATH_LOGIN = "user/login";
        private const string PATH_IDENTITY = "user/idcard/validate";
        private const string PATH_HEARTBEAT = "user/heartbeat";
        private const string PATH_LOGOUT = "logout";
        // private const string PATH_REGISTER = "account";
        private const string PATH_REGISTER = "user/register";
        private const string PATH_UNREGISTER = "user/clear";
        private const string PATH_UPLOAD_ONLINE_DATA = "heartbeat";
        
        private const string PATH_REQUEST_SMS = "user/captcha";
        private const string PATH_VALIDATE_SMS = "validateSmsCode";
        private const string PATH_GET_IAP_RECORDS = "user/payment";// 充值记录
        
        private const string TEMP_PAY = "iap/verify/receipt";
        
        private const string PATH_REFRESH_TOKEN = "osssts";

        //用户数据部分
        private const string DATA_UPLOAD = "user/sync";
        private const string DATA_DOWNLOAD = "user/data";
        private const string CLOUD_CONFIG = "config"; // 云控
        
        //public string TGADistinctId { get { return  ThinkingAnalytics.ThinkingAnalyticsAPI.GetDistinctId(); } }
        
        public string DeviceId
        {
            get
            {
                string devcieId = SystemInfo.deviceUniqueIdentifier;
                if (string.IsNullOrEmpty(devcieId))
                {
                  
                    if (!PlayerPrefs.HasKey("--deviceid--"))
                    {
                        string randomID = System.Guid.NewGuid().ToString("N");
                        PlayerPrefs.SetString("--deviceid--",randomID);
                        HLogger.LogWarning("NewGuid");
                    }
                    return PlayerPrefs.GetString("--deviceid--");
                }
                return devcieId;
            }
        }

        public void ValidateIdentity(UserAccount account, Action<IdentityResponse> response)
        {
            IdentityRequest request = new IdentityRequest {
                clientData = CurrentClientInfo(),
                token = account.AccessToken,
                idCardName = account.RealName,
                idCardNumber = account.IdCard,
            };
            HabbyFramework.Network.Post(request,response,PATH_IDENTITY);
        }
        
        public void RegisterWithAccount(string userName, string password, Action<RegisterResponse> response)
        {
            RegisterRequest request = new RegisterRequest {
                clientData = CurrentClientInfo(),
                customSocialId = userName,
                customPassword = password,
                deviceId = DeviceId
            };
            HabbyFramework.Network.Post(request,response,PATH_REGISTER);
        }
        
        public void LoginWithAccount(string userName ,string password, Action<LoginResponse> response)
        {
            LoginRequest request = new LoginRequest {
                clientData = CurrentClientInfo(),
                accountType = UserAccount.ChannelNACA,
                socialId = userName,
                password = password,
                deviceId = DeviceId
            };
            HabbyFramework.Network.Post(request,response,PATH_LOGIN);
        }
        
        public void UpdateUserOnlineData(UserAccount account, List<UserOnlieSegment> segments, Action<SyncOnlineDataResponse> response)
        {
            SyncOnlineDataRequest request = new SyncOnlineDataRequest {
                
                clientData = CurrentClientInfo(),
                timestamp = DateTime.Now.Ticks,
                token = account.AccessToken,
                activeTimeSegments = segments,
            };
            HabbyFramework.Network.Post(request,response,PATH_HEARTBEAT);
        }
        private bool _clientDataDirty = true;
        
        private ClientData _clientJsonInfo;
        
        public ClientData CurrentClientInfo()
        {
            if (_clientDataDirty) {
                _clientJsonInfo = makeClientData();
            }
            return _clientJsonInfo;
        }
        
        private ClientData makeClientData()
        {               
            return new ClientData {
                deviceId = DeviceId,
                appBundle = Application.identifier ?? "com.habby.pd",
                appVersion = Application.version ?? "1.24.0",
                osVersion = SystemInfo.operatingSystem ?? "",
                appLanguage = /*AppLanguage ??*/ "zh-CN",
                systemLanguage = Application.systemLanguage.ToString() ?? "ChineseSimplified",
                deviceModel = SystemInfo.deviceModel ?? "",
                //advertisementId = AdvertismentId ?? "2020",
                //tgaDistinctId = TGADistinctId ?? "2020",
                countryCode = RegionInfo.CurrentRegion.EnglishName??"China",

#if UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_IPHONE
                    os =  1,
#else
                os =  2
#endif
                
            };  
        }
        
    }
}