using SDKFramework.Account.DataSrc;
using SDKFramework.Account.Net;
using SDKFramework.Message;
using SDKFramework.Utils;

namespace SDKFramework.Account.Utils
{
    public class AccountDataUtil
    {
        public static void ParseAndSaveAccount(LoginResponse response,string channel)
        {
            var data = response.data;
            AccountLog.Info($"ParseLoginAccountInfo token={data.token}, age={data.age}");

            var account = new UserAccount
            {
                IsAgreePrivacy = true,
                LoginChannel = channel,
                Age = data.age,
                AccessToken = data.token,
                AgeRange = data.validateIdentity == 1 
                    ? (UserAccount.AgeLevel)data.addictLevel 
                    : UserAccount.AgeLevel.Unknown,
                NickName = data.nickname ?? "",
                PhoneNumber = data.phone ?? "",
                UnionId = data.unionId ?? "",
                UID = data.userId ?? "",
                IsNewUser = data.isNewUser,
            };

            AccountLog.Info($"--- AgeRange: {account.AgeRange} ,age={data.age} validateIdentity= {data.validateIdentity}");

            if (!string.IsNullOrEmpty(data.serverTime))
            {
                TimerHelper.CorrectSysTime(data.serverTime);
            }

            AccountLog.Info($"--- unionId: {data.unionId}");
            AccountLog.Info($"--- totalPaymentAmount: {data.totalPaymentAmount} ,monthlyPaymentAmount={data.monthlyPaymentAmount} ,todayPaymentAmoun={data.todayPaymentAmount}");

            if (data.isNewUser)
            {
                AccountLog.Info("IsNewUser");
                HabbyFramework.Analytics.TGA_first_login_suc();
            }
            else
            {
                AccountLog.Info("IsNotNewUser");
            }

            account.ResetOnline(data.totalOnlineTime, data.todayOnlineTime);
            account.ResetExpense(data.totalPaymentAmount, data.monthlyPaymentAmount, data.todayPaymentAmount);

            HabbyFramework.Account.SetCurrentAccount(account);
        }
    }
}