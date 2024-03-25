using SDKFramework.Account;
using UnityEngine;

namespace SDKFramework.Account.AntiAddiction
{
    public class GAPPListener : MonoBehaviour
    {
#if USE_ANTIADDICTION
        private void OnEnable()
        {
            HLogger.Log("开始侦听未成年人游戏行为",Color.green);
            AccountModule.OnNoTimeLeft += NoTimeLeft;

            AccountModule.OnSingleExpenseOverRange += ExpenseOverRange;
            AccountModule.OnMonthlyExpenseOverRange += ExpenseOverRange;
        }

        private void OnDisable()
        {
            AccountModule.OnNoTimeLeft -= NoTimeLeft;

            AccountModule.OnSingleExpenseOverRange -= ExpenseOverRange;
            AccountModule.OnMonthlyExpenseOverRange -= ExpenseOverRange;
            HLogger.Log("取消侦听未成年人游戏行为",Color.red);
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