using System;
using SDKFramework.Account.Net;
using SDKFramework.Account.DataSrc;
using SDKFramework.Account.AntiAddiction;
using static SDKFramework.Account.DataSrc.UserAccount;

namespace SDKFramework.Account
{
    public partial class AccountModule
    {
        protected internal override void OnModuleInit()
        {
            base.OnModuleInit();
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
        }

        public void LocalValidateIdentity()
        {
            UserAccount account = CurrentAccount;
            if (CanLogin(account))
            {
                if (account.AgeRange != UserAccount.AgeLevel.Adult)
                    HabbyFramework.UI.OpenUI(UIViewID.LatencyTimeUI);
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
            {
                HabbyFramework.UI.OpenUI(UIViewID.AntiaddictionRulesUI);
            }
#endif
            callback(true);
            Login(account);
        }
        
        private void Login(UserAccount account)
        {
            AccountLog.Info($"Login,account={account?.AccessToken}, age={account?.AgeRange}");
            if (account == null) return;
            account.SaveLoginTime();
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

        private void Logout(int actionCode = 0)
        {
            AccountLog.Info($"Logout, account={CurrentAccount}");
            if (CurrentAccount != null)
            {
#if USE_ANTIADDICTION
                timeManager.StopTimeCounter(CurrentAccount);
#endif
                //TODO：这部分逻辑应该重新整理，版署可以这样做，线上切换账号时登出，不会在本地清掉当前账号数据存档
                // (actionCode == 0 ? (Action)Save : ClearCurrent)();
            }

            IsLogin = false;

            (actionCode == 0 ? OnUserLogout : OnShowLoginScene)?.Invoke();
            AccountLog.Info($"Logout ActionCode={actionCode}");
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

    }
}