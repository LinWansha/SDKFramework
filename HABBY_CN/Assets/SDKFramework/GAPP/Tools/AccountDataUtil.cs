using SDKFramework.Config;

namespace Habby.CNUser
{
    public class AccountDataUtil
    {
        public static UserAccount ParseLoginAccountInfo(LoginResponse response)
        {
            HLogger.LogFormat("ParseLoginAccountInfo token={0}, age={1}", response.token, response.data.age);
            // int applicableRange = (int)SDKExample.AppData.applicableRange;
            // if (response.data.age < applicableRange)
            // {
            //     HabbyFramework.UI.OpenUI(UIViewID.NotGameTimeUI,
            //         string.Format(AntiAddictionDisaplayText.UnderAgeRangeNotice, applicableRange));
            // }        //TODO:逻辑梳理和整改

            UserAccount account = new UserAccount()
            {
                AccessToken = response.data.token,
                // ExpireAt = TimerHelper.ParseToDateTime(response.data.tokenExpireTime).Ticks,
                LoginState = UserAccount.UserLoginState.Logedin,

                AgeRange = response.data.validateIdentity == 1
                    ? (UserAccount.AgeLevel)response.data.addictLevel
                    : UserAccount.AgeLevel.Unknown,

                NickName = string.IsNullOrEmpty(response.data.nickname) ? "" : response.data.nickname,
                // Debug.Log("STEP 200: DefenseLog Init：" + UserAccoutManager.Instance.CurrentAccount.LastHeartBeatAt);
                // LastHeartBeatAt = response.data.lastHeartBeatAt
            };

            HLogger.LogWarning("--- AgeRange:" + account.AgeRange.ToString() + ",age=" + response.data.age +
                               ",validateIdentity=" + response.data.validateIdentity);

            if (!string.IsNullOrEmpty(response.data.serverTime))
            {
                TimerHelper.CorrectSysTime(response.data.serverTime);
            }


            HLogger.LogWarning("--- unionId:" + response.data.unionId);

            if (!string.IsNullOrEmpty(response.data.phone))
            {
                account.PhoneNumber = response.data.phone;
            }


            if (!string.IsNullOrEmpty(response.data.unionId))
            {
                account.UnionId = response.data.unionId;
            }
            else
            {
                account.UnionId = "";
            }

            if (!string.IsNullOrEmpty(response.data.userId))
            {
                account.UID = response.data.userId;
            }
            else
            {
                account.UID = "";
            }


            account.ResetOnline(response.data.totalOnlineTime, response.data.todayOnlineTime);
            account.ResetExpense(response.data.totalPaymentAmount, response.data.monthlyPaymentAmount,
                response.data.todayPaymentAmount);
            HLogger.LogWarning("--- totalPaymentAmount:" + response.data.totalPaymentAmount + ",monthlyPaymentAmount=" +
                               response.data.monthlyPaymentAmount + ",todayPaymentAmoun=" +
                               response.data.todayPaymentAmount);
            return account;
        }
    }
}