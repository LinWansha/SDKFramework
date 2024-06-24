using SDKFramework.Account.DataSrc;
using SDKFramework.Account.Net;
using SDKFramework.Utils;

namespace SDKFramework.Account.Utils
{
    public class AccountDataUtil
    {
        public static UserAccount ParseLoginAccountInfo(LoginResponse response)
        {
            AccountLog.Info($" ParseLoginAccountInfo token={response.token}, age={response.data.age}");

            UserAccount account = new UserAccount()
            {
                AccessToken = response.data.token,

                AgeRange = response.data.validateIdentity == 1
                    ? (UserAccount.AgeLevel)response.data.addictLevel
                    : UserAccount.AgeLevel.Unknown,

                NickName = string.IsNullOrEmpty(response.data.nickname) ? "" : response.data.nickname,

                Age = response.data.age
            };

            AccountLog.Info(
                $"--- AgeRange: {account.AgeRange.ToString()} ,age={response.data.age} validateIdentity= {response.data.validateIdentity}");

            if (!string.IsNullOrEmpty(response.data.serverTime))
            {
                TimerHelper.CorrectSysTime(response.data.serverTime);
            }


            AccountLog.Info($"--- unionId: {response.data.unionId}");

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
            AccountLog.Info(
                $"--- totalPaymentAmount: {response.data.totalPaymentAmount} ,monthlyPaymentAmount={response.data.monthlyPaymentAmount} ,todayPaymentAmoun={response.data.todayPaymentAmount}");
            return account;
        }
    }
}