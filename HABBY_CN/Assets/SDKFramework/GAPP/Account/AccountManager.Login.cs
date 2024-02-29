using System.Collections;
using System.Collections.Generic;
using SDKFramework.Utils;
using UnityEngine;

namespace Habby.CNUser
{
    public partial class AccountManager : MonoSingleton<AccountManager>
    {
        public void onClearUserCache()
        {
            HLogger.LogWarning("--- onClearUserCache");
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

        private void onUserLogout()
        {
            HLogger.LogWarnFormat("--- onUserLogout try crash!");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            // Utils.ForceCrash(ForcedCrashCategory.Abort);
            Application.Quit();
#endif
        }

        public void onUserLogin()
        {
            HLogger.Log("onUserLogin登录成功");
            HabbyFramework.UI.OpenUI(UIViewID.LoginSuccessUI);
            HabbyFramework.UI.CloseUI(UIViewID.EntryUI);
        }


        void Start()
        {
            HLogger.Log("--- LoginManager Start");
        }

        public void ShowLoginScene()
        {
            HLogger.Log("showLoginScene");
            if (OnShowLoginScene != null) OnShowLoginScene();
        }

        public void CheckUser()
        {
            if (HasAccount)
            {
                UserAccount account = CurrentAccount;
                HLogger.LogFormat("LoginManager checkUser token={0}, state={1}", account.AccessToken, account.LoginState);
                switch (account.LoginState)
                {
                    case UserAccount.UserLoginState.Logout:
                        ShowLoginScene();
                        break;
                }
            }
            else
            {
                HLogger.Log("LoginManager checkUser has no account info");
                ShowLoginScene();
            }
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

            if (CanLogin(account))
            {
                if (account.AgeRange != UserAccount.AgeLevel.Adult)
                    HabbyFramework.UI.OpenUI(UIViewID.LatencyTimeUI);
                Login(account);
            }
        }

        public void ValidateIdentity(UserAccount account)
        {
            HabbyUserClient.Instance.ValidateIdentity(account, (response) =>
            {
                HLogger.LogFormat("ValidateIdentity code={0}", response.code);
                if (IdentityResponse.CODE_SUCCESS==response.code)
                {
                    account.AgeRange = (UserAccount.AgeLevel)response.data.addictLevel;
                    OnIdentitySuccess?.Invoke();

                    if (!CanLogin(account))
                    {
                        OnAntiAddictionResultLogin?.Invoke(false);
                        return;
                    }

                    if (account.AgeRange != UserAccount.AgeLevel.Adult)
                    {
                        HabbyFramework.UI.OpenUI(UIViewID.AntiaddictionRulesUI);
                    }
                
                    Login(account);
                
                    OnAntiAddictionResultLogin?.Invoke(true);
                    return;
                }
                OnIdentityFailed(response.code);
            });
        }


        public void LoginOrIdentify(UserAccount account)
        {
            HLogger.LogFormat("LoginOrIdentify token={0}, channel={1}, age={2}", account.AccessToken, account.LoginChannel,
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
                    HabbyFramework.UI.OpenUI(UIViewID.RealNameUI, account);
                }
                else
                {
                    if (!CanLogin(account)) return;
#if USE_ANTIADDICTION
                    if (account.AgeRange != UserAccount.AgeLevel.Adult)
                        HabbyFramework.UI.OpenUI(UIViewID.AntiaddictionRulesUI);
#endif
                    Login(account);
                }
            }
        }


        public bool CanLogin(UserAccount accout)
        {
#if USE_ANTIADDICTION

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

    }
}