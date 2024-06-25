using System;
using System.Text.RegularExpressions;
using AgeLevel=SDKFramework.Account.DataSrc.UserAccount.AgeLevel;

namespace SDKFramework.Account.Utils
{
     /// <summary>
    /// 定义 生日年龄性别 实体
    /// </summary>
    public class BirthdayAgeSex
    {
        public string Birthday { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
    }
    
    public class LocalIdentityUtil
    {
        public static bool IsChineseName(string input)
        {
            Regex regex = new Regex(@"^[\u4E00-\u9FA5]{2,4}$");
            return regex.IsMatch(input);
        }
        
        // 身份证号码中的系数
        private static int[] coefficients = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };

        // 身份证号码中余数与校验码的对应
        private static char[] checkCodes = { '1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2' };

        // 中国居民身份证校验码算法
        // 步骤如下：
        // 1.将身份证号码前面的17位数分别乘以不同的系数。从第一位到第十七位的系数分别为：7－9－10－5－8－4－2－1－6－3－7－9－10－5－8－4－2。
        // 2.将这17位数字和系数相乘的结果相加。
        // 3.用加出来和除以11，取余数。
        // 4.余数只可能有0－1－2－3－4－5－6－7－8－9－10这11个数字。其分别对应的最后一位身份证的号码为1－0－X－9－8－7－6－5－4－3－2。
        // 5.通过上面计算得知如果余数是3，第18位的校验码就是9。如果余数是2那么对应的校验码就是X，X实际是罗马数字10。
        // 例如：某男性的身份证号码为【53010219200508011x】， 我们看看这个身份证是不是合法的身份证。首先我们得出前17位的乘积和【(5*7)+(3*9)+(0*10)+(1*5)+(0*8)+(2*4)+(1*2)+(9*1)+(2*6)+(0*3)+(0*7)+(5*9)+(0*10)+(8*5)+(0*8)+(1*4)+(1*2)】是189，
        // 然后用189除以11得出的结果是189/11=17----2，也就是说其余数是2。最后通过对应规则就可以知道余数2对应的检验码是X。所以，可以判定这是一个正确的身份证号码。
        // 第一代身份证号码为15位。年份两位数字表示，没有校验码。
        public static bool IsValidIDCard(string idCardNumber)
        {
            // 一代身份证：
            if (idCardNumber.Length == 15)
            {
                AccountLog.Warn("15位身份证号码不检查校验码。");
                return true;
            }
            // 二代身份证：
            if (!Regex.IsMatch(idCardNumber, @"^\d{17}(\d|X|x)$"))//        ^\d{17}(\d|X|x)$            ^\d{17}(\d|X)$
            {
                AccountLog.Warn("无效的身份证号码格式。");
                return false;
            }

            int sum = 0;

            // 计算前17位数字与系数的乘积之和
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(idCardNumber[i].ToString()) * coefficients[i];
            }

            // 计算余数
            int remainder = sum % 11;

            // 对应校验码中的字符，比较第18位，并返回验证结果
            char resultCheckCode = checkCodes[remainder];
            bool isValid = idCardNumber[17].ToString().ToUpper() == resultCheckCode.ToString();

            if(isValid)
            {
                AccountLog.Info("这是一个正确的身份证号码。");
            }
            else
            {
                AccountLog.Warn("这是一个错误的身份证号码。");
            }

            return isValid;
        }
        public static AgeLevel ParseAgeLevel(int age)
        {
            if (age < 8) return AgeLevel.Under8;
            if (age < 16) return AgeLevel.Under16;
            if (age < 18) return AgeLevel.Under18;
            return AgeLevel.Adult;
        }
        public static BirthdayAgeSex GetBirthdayAgeSex(string idCard)
        {
            if (string.IsNullOrEmpty(idCard)||idCard.Length != 15 && idCard.Length != 18)
            {
                return null;
            }

            BirthdayAgeSex entity = new BirthdayAgeSex();
            string strSex = string.Empty;
            if (idCard.Length == 18)//处理18位的身份证号码从号码中得到生日和性别代码
            {
                entity.Birthday = idCard.Substring(6, 4) + "-" + idCard.Substring(10, 2) + "-" + idCard.Substring(12, 2);
                strSex = idCard.Substring(14, 3);
            }
            if (idCard.Length == 15)
            {
                entity.Birthday = "19" + idCard.Substring(6, 2) + "-" + idCard.Substring(8, 2) + "-" + idCard.Substring(10, 2);
                strSex = idCard.Substring(12, 3);
            }

            entity.Age = CalculateAge(entity.Birthday);//根据生日计算年龄
            if (int.Parse(strSex) % 2 == 0)//性别代码为偶数是女性奇数为男性
            {
                entity.Sex = "女";
            }
            else
            {
                entity.Sex = "男";
            }
            return entity;
        }
        
        /// <summary>
        /// 根据出生日期，计算精确的年龄
        /// </summary>
        /// <param name="birthDate">生日</param>
        /// <returns></returns>
        public static int CalculateAge(string birthDay)
        {
            DateTime birthDate = DateTime.Parse(birthDay);
            DateTime nowDateTime = DateTime.Now;
            int age = nowDateTime.Year - birthDate.Year;
            //再考虑月、天的因素
            if (nowDateTime.Month < birthDate.Month || (nowDateTime.Month == birthDate.Month && nowDateTime.Day < birthDate.Day))
            {
                age--;
            }
            return age;
        }
        
    }
}