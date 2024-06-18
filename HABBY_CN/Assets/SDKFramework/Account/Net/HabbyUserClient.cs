using System;
using System.Collections.Generic;
using SDKFramework.Utils;
using SDKFramework.Account.DataSrc;
using UnityEngine;

namespace SDKFramework.Account.Net
{
    public class HabbyUserClient : MonoSingleton<HabbyUserClient>
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
        private const string PATH_GET_IAP_RECORDS = "user/payment"; // 充值记录

        private const string TEMP_PAY = "iap/verify/receipt";

        private const string PATH_REFRESH_TOKEN = "osssts";

        //用户数据部分
        private const string DATA_UPLOAD = "user/sync";
        private const string DATA_DOWNLOAD = "user/data";
        private const string CLOUD_CONFIG = "config"; // 云控

        private void Awake()
        {
            Application.RequestAdvertisingIdentifierAsync(OnGetAdvertisementId);
        }

        public void ValidateIdentity(UserAccount account, Action<IdentityResponse> response)
        {
            IdentityRequest request = new IdentityRequest
            {
                clientData = CurrentClientInfo(),
                token = account.AccessToken,
                idCardName = account.RealName,
                idCardNumber = account.IdCard,
            };
            HabbyFramework.Network.Post(request, response, PATH_IDENTITY);
        }

        public void RequestSmsCode(string phoneNumber, Action<SendUserSmsCodeResponse> response)
        {
            SendUserSmsCodeRequest request = new SendUserSmsCodeRequest()
            {
                clientData = CurrentClientInfo(),
                timestamp = DateTime.Now.Ticks,
                phone = phoneNumber,
                accountType = "phone",
            };

            HabbyFramework.Network.Post(request, response, PATH_REQUEST_SMS);
        }

        public void LoginPhoneChannel(Action<LoginResponse> response, string phone, string phoneCode)
        {
            LoginRequest request = new LoginRequest
            {
                clientData = CurrentClientInfo(),
                timestamp = DateTime.Now.Ticks,
                accountType = UserAccount.ChannelPhone,
                phone = phone,
                captcha = phoneCode
            };
            Log.Info("--- try login req=" + request.ToString());
            HabbyFramework.Network.Post(request, response, PATH_LOGIN);
        }

        public void LoginAppleId(Action<LoginResponse> response, string appleUid, string token, object userInfo,
            string unitId)
        {
            LoginRequest request = new LoginRequest
            {
                clientData = CurrentClientInfo(),
                timestamp = DateTime.Now.Ticks,
                accountType = UserAccount.ChannelAppleId
            };
            if (!string.IsNullOrEmpty(unitId))
            {
                request.socialId = unitId;
                Log.Info("--- try login appleid req unitId=" + unitId);
            }
            else
            {
                request.appleUserId = appleUid;
                request.identityToken = token;
                Log.Info("--- try login appleid req appleUserId=" + request.appleUserId + ",identityToken=" +
                         request.identityToken);
            }

            if (userInfo != null)
            {
                request.user = new AppleIdUserInfo();
                Log.Info("--- try login appleid userInfo type=" + userInfo.GetType() + ",userinfo=" + userInfo);
                if ((userInfo as Dictionary<string, object>) != null)
                {
                    Dictionary<string, object> userInfoDic = userInfo as Dictionary<string, object>;

                    if (userInfoDic.ContainsKey("email"))
                    {
                        request.user.email = userInfoDic["email"] as string;
                    }

                    if (userInfoDic.ContainsKey("namePrefix"))
                    {
                        request.user.namePrefix = userInfoDic["namePrefix"] as string;
                    }

                    if (userInfoDic.ContainsKey("givenName"))
                    {
                        request.user.givenName = userInfoDic["givenName"] as string;
                    }

                    if (userInfoDic.ContainsKey("middleName"))
                    {
                        request.user.middleName = userInfoDic["middleName"] as string;
                    }

                    if (userInfoDic.ContainsKey("familyName"))
                    {
                        request.user.familyName = userInfoDic["familyName"] as string;
                    }

                    if (userInfoDic.ContainsKey("nameSuffix"))
                    {
                        request.user.nameSuffix = userInfoDic["nameSuffix"] as string;
                    }

                    if (userInfoDic.ContainsKey("nickname"))
                    {
                        request.user.nickname = userInfoDic["nickname"] as string;
                    }
                }
                else
                {
                    Log.Info("--- userinfo as Dictionary<string, object> is null");
                }
            }
            else
            {
                Log.Info("--- try login appleid userInfo is nil");
            }


            HabbyFramework.Network.Post(request, response, PATH_LOGIN);
        }

        public void LoginQQChannel(Action<LoginResponse> response, string token)
        {
            LoginRequest request = new LoginRequest
            {
                clientData = CurrentClientInfo(),
                timestamp = DateTime.Now.Ticks,
                accountType = UserAccount.ChannelQQ,
                accessToken = token
            };
            HabbyFramework.Network.Post(request, response, PATH_LOGIN);
        }

        public void LoginPhoneQuickChannel(Action<LoginResponse> response, string token)
        {
            LoginRequest request = new LoginRequest
            {
                clientData = CurrentClientInfo(),
                timestamp = DateTime.Now.Ticks,
                accountType = UserAccount.ChannelPhone, // 登陆时还是phone，只是改为传mobileToken
                mobileToken = token
            };
            HabbyFramework.Network.Post(request, response, PATH_LOGIN);
        }


        /// <summary>
        ///  用本地存储的token登陆
        /// </summary>
        public void LoginWithToken(Action<LoginResponse> response, string channelName, string token = null)
        {
            LoginRequest request = new LoginRequest
            {
                clientData = CurrentClientInfo(),
                timestamp = DateTime.Now.Ticks,
                accountType = channelName,
                token = token
            };
            HabbyFramework.Network.Post(request, response, PATH_LOGIN);
        }

        public void LoginOAuthChannel(Action<LoginResponse> response, string channelName, string code = null, string openId = null)
        {
            if (string.IsNullOrEmpty(channelName))
            {
                Log.Info("--- LoginOAuthChannel param error! channelName is null!");
                return;
            }

            LoginRequest request = new LoginRequest
            {
                clientData = CurrentClientInfo(),
                timestamp = DateTime.Now.Ticks,
                accountType = channelName
            };
            if (!string.IsNullOrEmpty(code))
            {
                request.thirdpartyCode = code;
            }
            else if (!string.IsNullOrEmpty(openId))
            {
                request.socialId = openId;
            }
            else
            {
                throw new NotImplementedException("--- not implementException channel=" + channelName);
            }

            if (!string.IsNullOrEmpty(openId))
            {
                request.openid = openId;
            }

            Log.Info("--- try login req=" + request.ToString());
            HabbyFramework.Network.Post(request, response, PATH_LOGIN);
        }

        public void LoginWechat(Action<LoginResponse> response, string code = null, string openId = null)
        {
            LoginRequest request = new LoginRequest
            {
                clientData = CurrentClientInfo(),
                timestamp = DateTime.Now.Ticks,
                accountType = UserAccount.ChannelWeiXin
            };
            if (!string.IsNullOrEmpty(code))
            {
                request.thirdpartyCode = code;
            }
            else if (!string.IsNullOrEmpty(openId))
            {
                request.socialId = openId;
            }
            else
            {
                Log.Warn("--- LoginWechat param error! code and openId are alll null!");
            }

            Log.Info("--- ### LoginWechat: code=" + code + " openId=" + openId);
            HabbyFramework.Network.Post(request, response, PATH_LOGIN);
        }
        
        public void UnRegisterAccount(string tokenValue,string accountName,string codeValue,Action<UnregistAccountResponse> response)
        {
            UnregistAccountRequest request = new UnregistAccountRequest() {
                clientData = CurrentClientInfo(),
                timestamp = DateTime.Now.Ticks,
                code= codeValue,
                token = tokenValue,
                accountType = accountName
            };
            HabbyFramework.Network.Post(request, response,  PATH_UNREGISTER);
        }

        public void RegisterWithAccount(string userName, string password, Action<RegisterResponse> response)
        {
            RegisterRequest request = new RegisterRequest
            {
                clientData = CurrentClientInfo(),
                customSocialId = userName,
                customPassword = password,
                deviceId = DeviceId
            };
            HabbyFramework.Network.Post(request, response, PATH_REGISTER);
        }

        public void LoginWithAccount(string userName, string password, Action<LoginResponse> response)
        {
            LoginRequest request = new LoginRequest
            {
                clientData = CurrentClientInfo(),
                accountType = UserAccount.ChannelNACA,
                socialId = userName,
                password = password,
                deviceId = DeviceId
            };
            HabbyFramework.Network.Post(request, response, PATH_LOGIN);
        }

        public void UpdateUserOnlineData(UserAccount account, List<UserOnlieSegment> segments,
            Action<SyncOnlineDataResponse> response)
        {
            SyncOnlineDataRequest request = new SyncOnlineDataRequest
            {
                clientData = CurrentClientInfo(),
                timestamp = DateTime.Now.Ticks,
                token = account.AccessToken,
                activeTimeSegments = segments,
            };
            HabbyFramework.Network.Post(request, response, PATH_HEARTBEAT);
        }

        #region ClientData

        public void ResetDeviceData()
        {
            _clientDataDirty = true;
        }

        private ClientData makeClientData()
        {
            return new ClientData
            {
                deviceId = DeviceId,
                appBundle = Application.identifier ?? "com.hb.cnzzdr2.and",
                appVersion = Application.version ?? "1.24.0",
                osVersion = SystemInfo.operatingSystem ?? "",
                appLanguage = AppLanguage ?? "zh-CN",
                systemLanguage = Application.systemLanguage.ToString() ?? "ChineseSimplified",
                deviceModel = SystemInfo.deviceModel ?? "",
                advertisementId = AdvertisementId ?? "2020",
                tgaDistinctId = TGADistinctId ?? "2020",
                channelId = ChannelId,
                packageId = SubChannelId,
                appLocalVersion = Application.version,

#if UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_IPHONE
                    os = 1,
                    teamId = "",
                    bundleId = "",
#else
                os = 2
#endif
            };
        }

        private bool _clientDataDirty = true;
        private ClientData _clientJsonInfo;

        private ClientData CurrentClientInfo()
        {
            if (_clientDataDirty)
            {
                _clientJsonInfo = makeClientData();
            }

            return _clientJsonInfo;
        }

        public string AppLanguage;

        private string AdvertisementId;

        public string TGADistinctId;

        private string DeviceId
        {
            get
            {
                string deviceId = SystemInfo.deviceUniqueIdentifier;
                if (string.IsNullOrEmpty(deviceId))
                {
                    if (!PlayerPrefs.HasKey("DeviceId"))
                    {
                        string randomID = System.Guid.NewGuid().ToString("N");
                        PlayerPrefs.SetString("DeviceId", randomID);
                        Log.Warn("NewGuid");
                    }

                    return PlayerPrefs.GetString("DeviceId");
                }

                return deviceId;
            }
        }

        public int ChannelId; //todo: fill this by native

        private int SubChannelId
        {
            get
            {
                if (AppSource.Platform == RuntimePlatform.Android)
                {
                    return default; //todo: fill this by native
                }

                return 0;
            }
        }


        private void OnGetAdvertisementId(string adId, bool trackingEnabled, string errorMsg)
        {
            if (errorMsg != null)
            {
                Log.Info(errorMsg);
                AdvertisementId = TRACK_DISABLED;
                return;
            }

            AdvertisementId = trackingEnabled ? adId : TRACK_DISABLED;
        }

        #endregion
    }
}