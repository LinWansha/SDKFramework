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
            Log.Info(message:
                $" Reload data UID={CurrentAccount?.UID}" +
                $" TotalIAP={CurrentAccount?.IAP?.Total}" +
                $" TodayOnline={CurrentAccount?.Online?.Today}"+
                $" AccessToken={CurrentAccount?.AccessToken}");
        }
        
        public void SetPrivacyStatus(bool isAgree)
        {
            CurrentAccount.IsAgreePrivacy = isAgree;
            AccountLog.Info($"Privacy Status Change === {isAgree}");
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
                //TODO���ⲿ���߼�Ӧ������������������������������л��˺�ʱ�ǳ��������ڱ��������ǰ�˺����ݴ浵
                (actionCode == 0 ? (Action)Save : ClearCurrent)();
            }

            IsLogin = false;

            (actionCode == 0 ? OnUserLogout : OnShowLoginScene)?.Invoke();
            AccountLog.Info($"Logout ActionCode={actionCode}");
        }

        public void LocalValidateIdentity(UserAccount account)
        {
            OnIdentitySuccess?.Invoke();

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
                AccountLog.Info($"ValidateIdentity, code={response.code}");
                if (IdentityResponse.CODE_SUCCESS == response.code)
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

        public void LoginOrIdentify(Action<bool> callback)
        {
            UserAccount account = CurrentAccount;
            AccountLog.Info($"LoginOrIdentify, token={account.AccessToken}, channel={account.LoginChannel}, age={account.AgeRange}");
            if (string.IsNullOrEmpty(account.LoginChannel))
            {
                ShowLoginScene();
                return;
            }

            if (account.AgeRange == UserAccount.AgeLevel.Unknown)
            {
                HabbyFramework.UI.OpenUI(UIViewID.RealNameUI, account);
                return;
            }

#if USE_ANTIADDICTION
            if (!CanLogin(account)) 
                return;
            if (account.AgeRange != UserAccount.AgeLevel.Adult)
                HabbyFramework.UI.OpenUI(UIViewID.AntiaddictionRulesUI);
#endif

            Login(account);
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