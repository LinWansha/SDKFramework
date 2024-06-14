using SDKFramework.Account;
using UnityEngine;

namespace SDKFramework.Account.AntiAddiction
{
    public class GAPPListener : MonoBehaviour
    {
#if USE_ANTIADDICTION
        private void OnEnable()
        {
            AccountLog.Info("开始侦听未成年人游戏行为");
            AccountModule.OnNoTimeLeft += NoTimeLeft;

            AccountModule.OnSingleExpenseOverRange += ExpenseOverRange;
            AccountModule.OnMonthlyExpenseOverRange += ExpenseOverRange;
        }

        private void OnDisable()
        {
            AccountModule.OnNoTimeLeft -= NoTimeLeft;

            AccountModule.OnSingleExpenseOverRange -= ExpenseOverRange;
            AccountModule.OnMonthlyExpenseOverRange -= ExpenseOverRange;
            AccountLog.Info("取消侦听未成年人游戏行为");
        }
        
        private void ExpenseOverRange(LimitType limitType)
        {
            HabbyFramework.UI.OpenUI(UIViewID.PurchaseLimitUI, limitType);
        }


        private void NoTimeLeft()
        {
            HabbyFramework.UI.OpenUISingle(UIViewID.CrashUI,ExitReason.NoTimeLeft);
        }
#endif
    }
}