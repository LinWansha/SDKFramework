using System;
using SDKFramework.Account.AntiAddiction;
using SDKFramework.Account.DataSrc;
using SDKFramework.Account.Net;
using UnityEngine;
using static SDKFramework.Account.DataSrc.UserAccount;

namespace SDKFramework.Account
{
    public partial class AccountModule
    {
        public void Login(UserAccount account)
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

        public void Logout(int actionCode = 0)
        {
            AccountLog.Info($"Logout, account={CurrentAccount}");
            if (CurrentAccount != null)
            {
#if USE_ANTIADDICTION
                timeManager.StopTimeCounter(CurrentAccount);
#endif
                //TODO：这部分逻辑应该重新整理，版署可以这样做，线上切换账号时登出，不会在本地清掉当前账号数据存档
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

        public void LoginOrIdentify(UserAccount account)
        {
            AccountLog.Info($"LoginOrIdentify, token={account.AccessToken}, channel={account.LoginChannel}, age={account.AgeRange}");
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

        public bool CanLogin(UserAccount account)
        {
#if USE_ANTIADDICTION
            ExitReason? reason =
                NoRightAge(account) ? ExitReason.NoRightAge :
                NoGameTime(account) ? ExitReason.NoGameTime :
                NoTimeLeft(account) ? ExitReason.NoTimeLeft :
                (ExitReason?)null;

            if (reason != null)
            {
                HabbyFramework.UI.OpenUI(UIViewID.CrashUI, reason.Value);
            }

            return reason == null;
#else
            return true;
#endif
        }
    }
}