using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Habby.CNUser
{
    public class HabbyAntiAddictionExpenseListener : MonoBehaviour
    {
        private void OnEnable()
        {
            Debug.Log("1");
            AccountManager.OnExpenseOverRange += noticePurchaseAmountOverRange;
            AccountManager.OnMonthlyExpenseOverRange += noticeMonthlyExpenseOverRange;
        }

        private void OnDisable()
        {
            AccountManager.OnExpenseOverRange -= noticePurchaseAmountOverRange;
            AccountManager.OnMonthlyExpenseOverRange -= noticeMonthlyExpenseOverRange;
        }


        private void noticePurchaseAmountOverRange(UserAccount account)
        {
            HabbyFramework.UI.OpenUI(UIViewID.PurchaseLimitUI, account);
        }

        private void noticeMonthlyExpenseOverRange(UserAccount account)
        {
            HabbyFramework.UI.OpenUI(UIViewID.PurchaseLimitUI, account);
        }
    }
}