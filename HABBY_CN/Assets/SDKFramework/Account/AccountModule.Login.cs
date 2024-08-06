using System;
using SDKFramework.Account.Net;
using SDKFramework.Account.DataSrc;
using SDKFramework.Account.AntiAddiction;
using SDKFramework.Message;
using Sdkhubv2.Runtime.tools;
using static SDKFramework.Account.DataSrc.UserAccount;

namespace SDKFramework.Account
{
    public partial class AccountModule
    {
        protected internal override void OnModuleInit()
        {
            base.OnModuleInit();
            AccountLog.Info("AccountModule Init");
            RefreshLoginSessionId();
            Reload();
        }

        private void Reload()
        {
            if (HasAccount) return;
            CurrentAccount = FileSaveLoad.LoadAccount();
            AccountLog.Info(message:
                $" Reload data UID={CurrentAccount.UID}" +
                $" LoginChannel={CurrentAccount.LoginChannel}" +
                $" LoginSessionId={LoginSessionId}" +
                $" AgeRange={CurrentAccount.AgeRange}" +
                $" TotalIAP={CurrentAccount.IAP?.Total}" +
                $" TodayOnline={CurrentAccount.Online?.Today}"+
                $" AccessToken={CurrentAccount.AccessToken}");
        }
        
        public void SetPrivacyStatus(bool isAgree)
        {
            CurrentAccount.IsAgreePrivacy = isAgree;
            AccountLog.Info($"Privacy Status Change === {isAgree}");
            if (isAgree)HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.agree_privacy);
        }

        public void LocalValidateIdentity()
        {
            UserAccount account = CurrentAccount;
            if (CanLogin(account))
            {
                OnValidateIdentityResult?.Invoke(true,0);
                return;
            }
            OnValidateIdentityResult?.Invoke(false,-1);
        }
        
        public void StartValidation(Action<bool,int> callback)
        {
            UserAccount account = CurrentAccount;
            AccountLog.Info($"StartValidation, token={account.AccessToken}, channel={account.LoginChannel}, age={account.AgeRange}");
            if (string.IsNullOrEmpty(account.LoginChannel))
            {
                callback(false,-999);
                ShowLoginScene();
                return;
            }

            if (account.AgeRange == UserAccount.AgeLevel.Unknown)
            {
                AccountModule.OnValidateIdentityResult += callback;
                HabbyFramework.UI.OpenUI(UIViewID.RealNameUI);
                return;
            }
            OnValidateIdentityResult?.Invoke(true,0);
            callback(true, 0);
            // HabbyFramework.Analytics.TGA_first_active(); 2024.7.24 first_active不应该在这里调用（这里是登陆后不需要实名触发）
        }

        public void ValidateIdentity()
        {
            AccountLog.Info("ValidateIdentity");
            UserAccount account = CurrentAccount;
            HabbyUserClient.Instance.ValidateIdentity(account, (response) =>
            {
                AccountLog.Info($"ValidateIdentity response : code={response.code}");
                if (IdentityResponse.CODE_SUCCESS == response.code)
                {
                    HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.verify_success);
                    account.AgeRange = (UserAccount.AgeLevel)response.data.addictLevel;
                    OnValidateIdentityResult?.Invoke(true,0);
                    return;
                }
                HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.verify_fail);
                OnValidateIdentityResult?.Invoke(false,response.code);
            });
        }

        public void RealNameLogin(Action<bool> callback)
        {
            UserAccount account = CurrentAccount;
            AccountLog.Info($"RealNameLogin, AgeRange ={account.AgeRange}");
#if USE_ANTIADDICTION
            if (!CanLogin(account))
            {
                callback(false);
                return;
            }

            if (account.AgeRange != UserAccount.AgeLevel.Adult)
                HabbyFramework.UI.OpenUI(UIViewID.AntiaddictionRulesUI);
            else
                HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.age_pass);
#endif
            LoginDone(account);
            callback(true);
        }
        
        private void LoginDone(UserAccount account)
        {
            AccountLog.Info($"Login,account={account?.AccessToken}, age={account?.AgeRange}");
            
            if (account == null) return;
            CurrentAccount = account;
            Save();
            RefreshLoginSessionId();
#if USE_ANTIADDICTION
            account.IAP?.Refresh();
            account.Online?.Refresh();
            timeManager.StartTimeCounter();
            _ = (CurrentAccount.AgeRange != AgeLevel.Adult) ? gameObject.AddComponent<GAPPListener>() : null;
#endif
            IsLogin = true;
            CurrentAccount.IsLogin = true;
            OnUserLogin?.Invoke();
        }

        public void Login(Action<int, string> onResult,Action<int, string> onError)
        {
            HabbyFramework.Message.Subscribe<SDKEvent.SDKLoginFinish>(OnLoginFinish);

            void OnLoginFinish(SDKEvent.SDKLoginFinish arg)
            {
                onResult(arg.code, arg.msg);
            }
           
            try
            {
                #if ENABLE_DEBUG || DEBUG_MODEL
                AccountLog.Info("#AccountModule isLogin:" + CurrentAccount.IsLogin + " HasAccount:" + HasAccount + " AgeRange:" + CurrentAccount.AgeRange.ToString() );
                #endif
                var currentAccount = CurrentAccount;
                if ((!currentAccount.IsLogin && HasAccount) ||
                    currentAccount.AgeRange != UserAccount.AgeLevel.Adult &&
                    currentAccount.AgeRange != UserAccount.AgeLevel.Unknown)
                {
                    try
                    {
                        HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.login_choose_show);
                    }
                    catch (Exception e)
                    {
                    }
                    HabbyFramework.UI.OpenUI(UIViewID.QuickLoginUI);
                    return;
                }

                if (!HasAccount)
                {
                    try
                    {
                        HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.login_choose_show);
                    }
                    catch (Exception e)
                    {
                    }

                    if (ShanYanUtil.IsShanYanValid())
                    {
                        loginRunner.Execute(LoginChannel.PhoneQuick);
                    }
                    else
                    {
                        HabbyFramework.UI.OpenUI(UIViewID.LoginUI);
                    }
                }
                else
                    loginRunner.Execute(LoginChannel.History);
            }
            catch (Exception e)
            {
                onError.Invoke(ErrorCode.LOGIN_EXCEPTION, e.Message);
                throw;
            }
            
        }

        public void Logout(int actionCode = 0)
        {
            AccountLog.Info($"Logout,ActionCode={actionCode}, account={CurrentAccount}");
            
            if (CurrentAccount == null)return;
            IsLogin = false;
            CurrentAccount.IsLogin = false;
            RefreshLoginSessionId();
#if USE_ANTIADDICTION
            timeManager.StopTimeCounter(CurrentAccount);
#endif
            OnShowLoginScene?.Invoke();
            (actionCode == 0 ? OnUserLogout : OnShowLoginScene)?.Invoke();
            HabbyFramework.Message.Post(new SDKEvent.AccountLogout());
        }
        
        public void CheckUser()
        {
            if (HasAccount)
            {
                UserAccount account = CurrentAccount;
                AccountLog.Info($"checkUser token={account.AccessToken}");
                if (!IsLogin) ShowLoginScene();
            }
            else
            {
                AccountLog.Info($"checkUser has no account info");
                ShowLoginScene();
            }
        }

        public void UnRegister(Action<int,UnregistAccountResponse,string> callback)
        {
            var account = CurrentAccount;
            string oauthCode = "";
            string loginType = account.LoginChannel;
            if (ChannelPhoneQuick == loginType)
            {
                loginType = ChannelPhone;
            }
            if (account.LoginChannel == ChannelAppleId)
            {
                
            }
            
            HabbyUserClient.Instance.UnRegisterAccount(account.AccessToken,loginType,oauthCode, (response) =>
            {
                if (0 == response.code)
                {
                    AccountLog.Info("unregister user successful");
                    ClearCurrent();
                }
                else
                {
                    AccountLog.Info("unregister user failure");
                }

                callback(response.code, response, "");
            });
        }
    }
}