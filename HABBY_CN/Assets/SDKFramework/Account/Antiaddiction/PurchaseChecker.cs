using SDKFramework.Account.DataSrc;
using static SDKFramework.Account.DataSrc.UserAccount;

namespace SDKFramework.Account.AntiAddiction
{
    public static class PurchaseChecker
    {
        //单次支付金额限制, 以分为单位
        const int OnceAmountLimit_U16 = 5000;
        const int OnceAmountLimit_U18 = 10000;

        //总支付金额限制, 以分为单位
        public const int MonthlyAmountLimit_U16 = 20000;
        public const int MonthlyAmountLimit_U18 = 40000;

        //总支付金额限制, 以分为单位
        const int TotalAmountLimit_U16 = int.MaxValue;
        const int TotalAmountLimit_U18 = int.MaxValue;

        //每日抽宝箱次数限制
        public const int DAILY_GACHA_LIMIT = 30;

        /// <summary>
        /// 支付校验（非支付时）
        /// </summary>
        /// <returns></returns>
        public static bool CanPurchase(UserAccount account, double money)
        {
            if (account == null) {
                HabbyFramework.Account.FireExpenseOverRange();
                return false;
            }
            // account.AgeRange = AgeLevel.Under8;
            switch (account.AgeRange)
            {
                case AgeLevel.Unknown:
                    HabbyFramework.Account.FireExpenseOverRange();
                    return false;
                case AgeLevel.Under8:
                    HabbyFramework.Account.FireExpenseOverRange();
                    return false;
                case AgeLevel.Under16:
                    if (money > OnceAmountLimit_U16) {
                        HabbyFramework.Account.FireExpenseOverRange();
                        return false;
                    } else if (account.IAP != null && account.IAP.Monthly + money > MonthlyAmountLimit_U16) {
                        HabbyFramework.Account.FireMonthlyExpenseOverRange();
                        return false;
                    }
                    break;
                case AgeLevel.Under18:
                    if (money > OnceAmountLimit_U18) {
                        HabbyFramework.Account.FireExpenseOverRange();
                        return false;
                    } else {
                        if (account.IAP != null && account.IAP.Monthly + money > MonthlyAmountLimit_U18) {
                            HabbyFramework.Account.FireMonthlyExpenseOverRange();
                            return false;
                        }
                    }
                    break;
            }

            return true;
        }
        
    }
}
