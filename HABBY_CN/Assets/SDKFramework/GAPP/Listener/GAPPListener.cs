using UnityEngine;

namespace Habby.CNUser
{
    public class GAPPListener : MonoBehaviour
    {
#if USE_ANTIADDICTION
        private void OnEnable()
        {
            HLogger.Log("开始侦听未成年人游戏行为",Color.green);
            AccountManager.OnNoTimeLeft += NoTimeLeft;

            AccountManager.OnSingleExpenseOverRange += ExpenseOverRange;
            AccountManager.OnMonthlyExpenseOverRange += ExpenseOverRange;
        }

        private void OnDisable()
        {
            AccountManager.OnNoTimeLeft -= NoTimeLeft;

            AccountManager.OnSingleExpenseOverRange -= ExpenseOverRange;
            AccountManager.OnMonthlyExpenseOverRange -= ExpenseOverRange;
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