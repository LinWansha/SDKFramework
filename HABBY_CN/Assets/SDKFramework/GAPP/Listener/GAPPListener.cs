using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Habby.CNUser
{
    public class GAPPListener : MonoBehaviour
    {
        private void OnEnable()
        {
            Debug.Log("1");
            AccountManager.OnNoTimeLeft += NoTimeLeft;

            AccountManager.OnSingleExpenseOverRange += ExpenseOverRange;
            AccountManager.OnMonthlyExpenseOverRange += ExpenseOverRange;
        }

        private void OnDisable()
        {
            AccountManager.OnNoTimeLeft -= NoTimeLeft;

            AccountManager.OnSingleExpenseOverRange -= ExpenseOverRange;
            AccountManager.OnMonthlyExpenseOverRange -= ExpenseOverRange;
        }
        
        private void ExpenseOverRange(LimitType limitType)
        {
            HabbyFramework.UI.OpenUI(UIViewID.PurchaseLimitUI, limitType);
        }


        private void NoTimeLeft()
        {
            HabbyFramework.UI.OpenUI(UIViewID.NoTimeLeftUI);
        }
    }
}