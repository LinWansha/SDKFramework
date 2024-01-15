using Habby.CNUser;
using System;
using System.Collections.Generic;
using System.Globalization;

/// <summary>
/// 节假日检测类By:zmoli775
/// </summary>
public class JJR
{
    public const int UNKNOWN = 0;
    public const int WORKDAY = 1;
    public const int HOLIDAY = 2;
    public List<string>
        // 2020年法定节假日
        y2020 = new List<string>(),
        y2021 = new List<string>(),
        y2022 = new List<string>(),
        y2023 = new List<string>(),
        y2024 = new List<string>(),
        y2025 = new List<string>();
    /// <summary>
    /// 节假日检测(仅支持判断2019年、2020年)
    /// </summary>
    public JJR()
    {
        /*2020年有效数据*/
        y2020.Add("20200101"); // 元旦
        y2020.Add("20200125"); y2020.Add("20200126"); y2020.Add("20200127");// 春节
        y2020.Add("20200404"); // 清明
        y2020.Add("20200501"); // 劳动
        y2020.Add("20200625"); // 端午
        y2020.Add("20201001"); y2020.Add("20201002"); y2020.Add("20201003"); // 国庆


        /*2021年有效数据*/
        y2021.Add("20210101"); // 元旦
        y2021.Add("20210212"); y2021.Add("20210213"); y2021.Add("20210214");// 春节
        y2021.Add("20210404"); // 清明
        y2021.Add("20210501"); // 劳动
        y2021.Add("20210614"); // 端午
        y2021.Add("20210921"); // 重阳
        y2021.Add("20211001"); y2021.Add("20211002"); y2021.Add("20211003"); // 国庆


        /*2022年有效数据*/
        y2022.Add("20220101"); // 元旦
        y2022.Add("20220201"); y2022.Add("20220202"); y2022.Add("20220203");// 春节
        y2022.Add("20220405"); // 清明
        y2022.Add("20220501"); // 劳动
        y2022.Add("20220603"); // 端午
        y2022.Add("20220910"); // 重阳
        y2022.Add("20221001"); y2022.Add("20221002"); y2022.Add("20221003"); // 国庆
        
        /*2023年有效数据*/
        y2023.Add("20230101"); // 元旦
        y2023.Add("20230204"); y2023.Add("20230205"); y2023.Add("20230206");// 春节
        y2023.Add("20230405"); // 清明
        y2023.Add("20230501"); // 劳动
        y2023.Add("20230622"); // 端午
        y2023.Add("20230929"); // 重阳
        y2023.Add("20231001"); y2023.Add("20231002"); y2023.Add("20231003"); // 国庆
        
        /*2024年有效数据*/
        y2024.Add("20240101"); // 元旦
        y2024.Add("20240210"); y2024.Add("20240211"); y2024.Add("20240212");// 春节
        y2024.Add("20240404"); // 清明
        y2024.Add("20240501"); // 劳动
        y2024.Add("20240610"); // 端午
        y2024.Add("20240917"); // 重阳
        y2024.Add("20241001"); y2024.Add("20241002"); y2024.Add("20241003"); // 国庆
        
        /*2025年有效数据*/
        y2025.Add("20250101"); // 元旦
        y2025.Add("20250129"); y2025.Add("20250130"); y2025.Add("20250131");// 春节
        y2025.Add("20250404"); // 清明
        y2025.Add("20250501"); // 劳动
        y2025.Add("20250531"); // 端午
        y2025.Add("20251001"); // 重阳
        y2025.Add("20251002"); y2025.Add("20251003"); y2025.Add("20251006"); // 国庆
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
            // 2020年
            case 2020:
                type = Check(y2020, date); break;
            case 2021:
                type = Check(y2021, date); break;
            case 2022:
                type = Check(y2022, date); break;
            case 2023:
                type = Check(y2023, date); break;
            case 2024:
                type = Check(y2024, date); break;
            case 2025:
                type = Check(y2025, date); break;
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
        foreach (var i in y) {
            if (IsSameDay(date, i)) return HOLIDAY;
        }

        //// 调休工作日检查
        //foreach (string[] i in w)
        //{
        //    foreach (var item in i)
        //    {
        //        if (date == Str2dt(item))
        //        {
        //            return WORKDAY;
        //        }
        //    }
        //}
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
