using System;
using System.Text;
using SDKFramework.Account.DataSrc;
using SDKFramework.Account.Net;
using SDKFramework.Utils;
using UnityEngine;

namespace SDKFramework.Account.Utils
{
    public class AccountDataUtil
    {
        public static UserAccount ParseLoginAccountInfo(LoginResponse response)
        {
            HLogger.Log($"[Account] ParseLoginAccountInfo token={response.token}, age={response.data.age}",
                Color.magenta);

            UserAccount account = new UserAccount()
            {
                AccessToken = response.data.token,

                AgeRange = response.data.validateIdentity == 1
                    ? (UserAccount.AgeLevel)response.data.addictLevel
                    : UserAccount.AgeLevel.Unknown,

                NickName = string.IsNullOrEmpty(response.data.nickname) ? "" : response.data.nickname,

                Age = response.data.age
            };

            HLogger.Log(
                $"[Account] --- AgeRange: {account.AgeRange.ToString()} ,age={response.data.age} validateIdentity= {response.data.validateIdentity}",
                Color.magenta);

            if (!string.IsNullOrEmpty(response.data.serverTime))
            {
                TimerHelper.CorrectSysTime(response.data.serverTime);
            }


            HLogger.Log($"[Account] --- unionId: {response.data.unionId}", Color.magenta);

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
            HLogger.Log(
                $"[Account] --- totalPaymentAmount: {response.data.totalPaymentAmount} ,monthlyPaymentAmount={response.data.monthlyPaymentAmount} ,todayPaymentAmoun={response.data.todayPaymentAmount}",
                Color.magenta);
            return account;
        }
        public static string Encode(string userId, string password)
        {
            string combined = userId + "|" + password;
            byte[] combinedBytes = Encoding.UTF8.GetBytes(combined);
            return Convert.ToBase64String(combinedBytes);
        }

        public static Tuple<string, string> Decode(string base64Encoded)
        {
            byte[] decodedBytes = Convert.FromBase64String(base64Encoded);
            string decoded = Encoding.UTF8.GetString(decodedBytes);
            string[] result = decoded.Split('|');
            return Tuple.Create(result[0], result[1]);
        }
    }
}