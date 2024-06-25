using System;
using SDKFramework.Account.Net;
using SDKFramework.Account.DataSrc;
using SDKFramework.Account.AntiAddiction;
using SDKFramework.Message;
using static SDKFramework.Account.DataSrc.UserAccount;

namespace SDKFramework.Account
{
    public partial class AccountModule
    {
        protected internal override void OnModuleInit()
        {
            base.OnModuleInit();
            AccountLog.Info("AccountModule Init");
            Reload();
        }

        private void Reload()
        {
            if (HasAccount) return;
            CurrentAccount = FileSaveLoad.LoadAccount();
            AccountLog.Info(message:
                $" Reload data UID={CurrentAccount.UID}" +
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
            HabbyFramework.Analytics.TGA_first_active();
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
                    OnValidateIdentityResult?.Invoke(false,0);
                    return;
                }
                
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
            Login(account);
            callback(true);
        }
        
        private void Login(UserAccount account)
        {
            AccountLog.Info($"Login,account={account?.AccessToken}, age={account?.AgeRange}");
            
            if (account == null) return;
            CurrentAccount = account;
            FileSaveLoad.SaveAccount(account);
            AccountHistory.SaveAccount(account);
            
#if USE_ANTIADDICTION
            account.IAP?.Refresh();
            account.Online?.Refresh();
            timeManager.StartTimeCounter();
            _ = (CurrentAccount.AgeRange != AgeLevel.Adult) ? gameObject.AddComponent<GAPPListener>() : null;
#endif
            IsLogin = true;
            OnUserLogin?.Invoke();
        }

        public void Logout(int actionCode = 0)
        {
            AccountLog.Info($"Logout,ActionCode={actionCode}, account={CurrentAccount}");
            if (CurrentAccount == null)return;

            IsLogin = false;
#if USE_ANTIADDICTION
            timeManager.StopTimeCounter(CurrentAccount);
#endif
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

        public void UnRegister(Action<bool> callback)
        {
            var account = CurrentAccount;
            string oauthCode = "";
            if (account.LoginChannel == ChannelAppleId)
            {
                //TODO: 删除ios授权前 先从新申请授权（为了拿到授权code）
                oauthCode = "删除ios授权前 先从新申请授权（为了拿到授权code）";
            }
            HabbyUserClient.Instance.UnRegisterAccount(account.AccessToken,account.LoginChannel,oauthCode, (response) =>
            {
                if (0==response.code)
                {
                    AccountLog.Info("注销账号成功");
                    Logout(1);
                    ClearCurrent();
                    callback(true);
                }
                else
                {
                    AccountLog.Info("注销账号失败");
                    callback(false);
                }
            });
        }
    }
}