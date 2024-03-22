using System;
using System.Collections.Generic;
using UnityEngine;

namespace Habby.CNUser
{
    [Serializable]
    public class UserExpenseData
    {
        private const string KEY_TOTAL_SPEND = "purchase_total";
        private const string KEY_MONTH_SPEND = "purchase_month_{0}";
        private const string KEY_DAY_SPEND = "purchase_day_{0}";

        public UserDoubleDataEntry totalExpense;
        public UserDoubleDataEntry monthlyExpense;
        public UserDoubleDataEntry dailyExpense;

        public UserExpenseData()
        {
            totalExpense = createTotal();
            monthlyExpense = createMontyly();
            dailyExpense = createDaily();
        }

        public void Refresh()
        {
            if (totalExpense == null) totalExpense = createTotal();
            DateTime now = DateTime.Now;
            string key = string.Format(KEY_MONTH_SPEND, now.Month);
            if (monthlyExpense == null || !key.Equals(monthlyExpense.name)) monthlyExpense = createMontyly(key);

            key = string.Format(KEY_DAY_SPEND, now.DayOfYear);
            if (dailyExpense == null || !key.Equals(dailyExpense.name)) dailyExpense = createDaily(key);
        }

        public void Add(double value)
        {
            DateTime now = DateTime.Now;
            string daily = string.Format(KEY_DAY_SPEND, now.DayOfYear);
            if (!daily.Equals(dailyExpense.name))
            {
                dailyExpense = createDaily(daily);

                string month = string.Format(KEY_MONTH_SPEND, now.Month);
                if (!month.Equals(monthlyExpense.name))
                {
                    monthlyExpense = createMontyly(month);
                }
            }
            
            totalExpense.value += value;
            monthlyExpense.value += value;
            dailyExpense.value += value;
        }

        public double Total => totalExpense.value;
        public double Monthly => monthlyExpense.value;
        public double Daily => dailyExpense.value;

        private static UserDoubleDataEntry createTotal()
        {
            return new UserDoubleDataEntry {
                name = KEY_TOTAL_SPEND,
                value = 0
            };
        }

        private static UserDoubleDataEntry createMontyly(string key = null)
        {
            if (key == null) {
                DateTime now = DateTime.Now;
                key = string.Format(KEY_MONTH_SPEND, now.Month);
            }
            return new UserDoubleDataEntry() {
                name = key,
                value = 0
            };
        }

        private static UserDoubleDataEntry createDaily(string key = null)
        {
            if (key == null) {
                DateTime now = DateTime.Now;
                key = string.Format(KEY_DAY_SPEND, now.DayOfYear);
            }
            return new UserDoubleDataEntry() {
                name = key,
                value = 0
            };
        }
    }
}
