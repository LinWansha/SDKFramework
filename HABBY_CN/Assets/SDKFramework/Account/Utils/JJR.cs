using System;
using System.Collections.Generic;

namespace SDKFramework.Account.Utils
{
    /// <summary>
    /// 节假日检测类
    /// </summary>
    public class JJR
    {
        public const int UNKNOWN = 0;
        public const int WORKDAY = 1;
        public const int HOLIDAY = 2;

        public List<string>
            // 2020年法定节假日
            y2024 = new List<string>(),
            y2025 = new List<string>(),
            y2026 = new List<string>(),
            y2027 = new List<string>(),
            y2028 = new List<string>();

        /// <summary>
        /// 节假日检测(仅支持判断2019年、2020年)
        /// </summary>
        public JJR()
        {
            /*2024年有效数据*/
            y2024.Add("20240101"); // 元旦
            y2024.Add("20240210");
            y2024.Add("20240211");
            y2024.Add("20240212"); // 春节
            y2024.Add("20240404"); // 清明
            y2024.Add("20240501"); // 劳动
            y2024.Add("20240610"); // 端午
            y2024.Add("20240917"); // 重阳
            y2024.Add("20241001");
            y2024.Add("20241002");
            y2024.Add("20241003"); // 国庆

            /*2025年有效数据*/
            y2025.Add("20250101"); // 元旦
            y2025.Add("20250129");
            y2025.Add("20250130");
            y2025.Add("20250131"); // 春节
            y2025.Add("20250404"); // 清明
            y2025.Add("20250501"); // 劳动
            y2025.Add("20250531"); // 端午
            y2025.Add("20251001"); // 重阳
            y2025.Add("20251002");
            y2025.Add("20251003");
            y2025.Add("20251006"); // 国庆

            /*2026年有效数据*/
            y2026.Add("20260101"); // 元旦
            y2026.Add("20260216");
            y2026.Add("20260217");
            y2026.Add("20260218"); // 春节
            y2026.Add("20260404"); // 清明
            y2026.Add("20260501"); // 劳动
            y2026.Add("20260519"); // 端午
            y2026.Add("20260926"); // 重阳
            y2026.Add("20261001");
            y2026.Add("20261002");
            y2026.Add("20261003"); // 国庆

            /*2027年有效数据*/
            y2027.Add("20270101"); // 元旦
            y2027.Add("20270205");
            y2027.Add("20270206");
            y2027.Add("20270207"); // 春节
            y2027.Add("20270404"); // 清明
            y2027.Add("20270501"); // 劳动
            y2027.Add("20270608"); // 端午
            y2027.Add("20271014"); // 重阳
            y2027.Add("20271001");
            y2027.Add("20271002");
            y2027.Add("20271003"); // 国庆

            /*2028年有效数据*/
            y2028.Add("20280101"); // 元旦
            y2028.Add("20280125");
            y2028.Add("20280126");
            y2028.Add("20280127"); // 春节
            y2028.Add("20280404"); // 清明
            y2028.Add("20280501"); // 劳动
            y2028.Add("20280528"); // 端午
            y2028.Add("20281003"); // 重阳
            y2028.Add("20281001");
            y2028.Add("20281002");
            y2028.Add("20281003"); // 国庆
        }

        public bool IsHoliday(DateTime date)
        {
            return CheckForHolidays(date) == HOLIDAY;
        }

        /// <summary>
        /// 检测日期状态(3:节假日、2:休息日、1:工作日、0:错误)
        /// </summary>
        /// <param name="rq">日期(20191231)</param>
        /// <returns>返回状态码(3:节假日、2:休息日、1:工作日、0:错误)</returns>
        public int CheckForHolidays(DateTime date)
        {
            return Determine(date);
        }

        private int Determine(DateTime date)
        {
            int type = 0;

            switch (date.Year)
            {
                case 2024:
                    type = Check(y2024, date);
                    break;
                case 2025:
                    type = Check(y2025, date);
                    break;
                case 2026:
                    type = Check(y2026, date);
                    break;
                case 2027:
                    type = Check(y2027, date);
                    break;
                case 2028:
                    type = Check(y2028, date);
                    break;
            }

            if (type == 0)
            {
                int week = Convert.ToInt32(date.DayOfWeek);
                if (6 == week || 0 == week || 5 == week) // 类型为0`检测是否为星期六、星期日、星期五
                {
                    type = HOLIDAY; // 休息日
                }
                else
                {
                    type = WORKDAY; // 工作日
                }
            }

            return type;
        }

        private int Check(List<string> y, DateTime date)
        {
            // 法定节假日检查
            foreach (var i in y)
            {
                if (IsSameDay(date, i)) return HOLIDAY;
            }

            return UNKNOWN;
        }

        /// <summary>
        /// string转换DateTime
        /// </summary>
        private static bool IsSameDay(DateTime date, string d)
        {
            int month = date.Month;
            int day = date.Day;

            int dm = int.Parse(d.Substring(4, 2));
            int dd = int.Parse(d.Substring(6, 2));

            return month == dm && day == dd;
        }
    }
}