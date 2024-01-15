using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Habby.CNUser
{
    public partial class AccountManager : MonoSingleton<AccountManager>
    {
        public void onClearUserCache()
        {
            HLog.LogWarning("--- onClearUserCache");
            AccountHistory.DeleteHistory();
            ClearCurrent();
        }



        void OnEnable()
        {
            AccountManager.OnUserLogin += onUserLogin;
            AccountManager.OnUserLogout += onUserLogout;
        }

        public void OnDisable()
        {
            AccountManager.OnUserLogin -= onUserLogin;
            AccountManager.OnUserLogout -= onUserLogout;
        }

        private void onUserLogout(UserAccount account)
        {
            HLog.LogWarnFormat("--- onUserLogout try crash!");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            // Utils.ForceCrash(ForcedCrashCategory.Abort);
            Application.Quit();
#endif
        }

        public void onUserLogin(UserAccount account)
        {
            HLog.Log("onUserLogin登录成功");
            HabbyFramework.UI.CloseUI(UIViewID.EntryUI);
        }


        void Start()
        {
            HLog.Log("--- LoginManager Start");
        }

        public void ShowLoginScene()
        {
            HLog.Log("showLoginScene");
            if (OnShowLoginScene != null) OnShowLoginScene();
        }

        public void CheckUser()
        {
            if (HasAccount)
            {
                UserAccount account = CurrentAccount;
                HLog.LogFormat("LoginManager checkUser token={0}, state={1}", account.AccessToken, account.LoginState);
                switch (account.LoginState)
                {
                    case UserAccount.UserLoginState.Logout:
                        ShowLoginScene();
                        break;
                }
            }
            else
            {
                HLog.Log("LoginManager checkUser has no account info");
                ShowLoginScene();
            }
        }


        public void LoginNative()
        {
            HabbyFramework.UI.OpenUI(UIViewID.LoginUI);
        }

        public void LocalValidateIdentity(UserAccount account)
        {
            if (OnIdentitySuccess != null) OnIdentitySuccess();

            BirthdayAgeSex entity = new BirthdayAgeSex();
            entity = LocalIdentityUtil.GetBirthdayAgeSex(account.IdCard);
            
            if (entity.Age >= 18)
            {
                account.AgeRange = UserAccount.AgeLevel.Adult;
            }

            if (entity.Age >= 8 && entity.Age < 16)
            {
                account.AgeRange = UserAccount.AgeLevel.Under16;
            }

            if (entity.Age >= 16 && entity.Age < 18)
            {
                account.AgeRange = UserAccount.AgeLevel.Under18;
            }

            if (entity.Age < 8)
            {
                account.AgeRange = UserAccount.AgeLevel.Under8;
            }

            // HabbyAntiAddictionPopupManager.Instance.CloseIndicator();
            if (CanLogin(account))
            {
                if (account.AgeRange != UserAccount.AgeLevel.Adult)
                    HabbyFramework.UI.OpenUI(UIViewID.LatencyTimeUI);
                Login(account);
            }
        }

        public void ValidateIdentity(UserAccount account)
        {
            // Debug.LogWarningFormat("--- ValidateIdentity age={0}",account.AgeRange);
            //
            // HabbyAntiAddictionPopupManager.Instance.OpenIndicator();
            // TGAManager.Instance.track_cn_verify_result("SUBMIT","SUCCESS",0,$"name={account.RealName},id={account.IdCard}");
            // HabbyUserClient.Instance.ValidateIdentity(account, (response) => {
            //     HabbyAntiAddictionPopupManager.Instance.CloseIndicator();
            //     HLog.LogFormat("ValidateIdentity code={0}", response.code);
            //     if  (IdentityResponse.CODE_SUCCESS == response.code )
            //     {
            //         account.AgeRange = (UserAccount.AgeLevel)response.data.addictLevel;
            //         Debug.LogWarningFormat("--- ValidateIdentity result AgeRange={0},age={1}",account.AgeRange,response.data.age);
            //         SDKHub.VerifyingAge = response.data.addictLevel;
            //         TGAManager.Instance.track_cn_verify_result("FINISH","SUCCESS",0,$"age={response.data.age},ageLv={account.AgeRange}");
            //         OnIdentitySuccess?.Invoke();
            //         
            //         if (!CanLogin(account))
            //         {
            //             OnAntiAddictionResultLogin?.Invoke(false);
            //             // TGAManager.Instance.track_cn_verify_result("SUBMIT","SUCCESS");
            //             return;
            //         }
            //         if (account.AgeRange != UserAccount.AgeLevel.Adult)
            //         {
            //             HabbyAntiAddictionPopupManager.Instance.OpenNoticeTeenager();
            //         }
            //         Login(account);
            //         OnAntiAddictionResultLogin?.Invoke(true);
            //         
            //     }
            //     else
            //     {
            //         if (response.code == IdentityResponse.ID_CARD_OVER_COUNT)
            //         {
            //            // TGAManager.Instance.track_cn_verify_result("FINISH","over_max",response.code,$"name={account.RealName},id={account.IdCard}");
            //         }
            //         else
            //         {
            //          //   TGAManager.Instance.track_cn_verify_result("FINISH","FAIL",response.code,$"name={account.RealName},id={account.IdCard}");
            //         }
            //        
            //         OnIdentityFailed(response.code);
            //     }
            //     
            // }, onLoginNetworkError);
        }

        private void onLoginNetworkError(string error)
        {
            HLog.LogFormat("Login failed network error {0}", error);
            HabbyFramework.UI.CloseUI(UIViewID.LatencyTimeUI);
        }


        public void LoginOrIdentify(UserAccount account)
        {
            HLog.LogFormat("LoginOrIdentify token={0}, channel={1}, age={2}", account.AccessToken, account.LoginChannel,
                account.AgeRange);
            // 不允许匿名登陆
            if (string.IsNullOrEmpty(account.LoginChannel))
            {
                ShowLoginScene();
            }
            else
            {
                if (account.AgeRange == UserAccount.AgeLevel.Unknown)
                {
                    Login(account);//初次登录没有实名的用户也先登录 TODO:与后端确认
                    HabbyFramework.UI.OpenUI(UIViewID.RealNameUI, account);
                }
                else
                {
                    if (!CanLogin(account)) return;
#if USE_ANTIADDICTION_TIME
                    if (account.AgeRange != UserAccount.AgeLevel.Adult)
                        HabbyFramework.UI.OpenUI(UIViewID.AntiaddictionRulesUI);
#endif
                    Login(account);
                }
            }
        }


        public bool CanLogin(UserAccount accout)
        {
#if USE_ANTIADDICTION_TIME

            if (IsRestrictedTime(accout))
            {
                HabbyFramework.UI.OpenUI(UIViewID.NotGameTimeUI);
                return false;
            }
            else if (!HasTimeLeft(accout))
            {
                HabbyFramework.UI.OpenUI(UIViewID.NoTimeLeftUI);
                return false;
            }
#endif
            return true;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            HLog.Log("LoginManager OnApplicationFocus");
        }
    }
}