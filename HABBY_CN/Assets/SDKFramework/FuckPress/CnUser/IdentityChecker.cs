using System;
using System.Text.RegularExpressions;

namespace Habby.CNUser
{
    public class IdentityChecker
    {
        const string TAG = "IdentityChecker";

        public static bool CheckPhoneNumber(string phoneNumber)
        {
            if (phoneNumber == null || phoneNumber.Length != 11)
            {
                return false;
            }
            //string[] firstcheck = new string[] {
            //    /*移动*/"134", "135", "136", "137", "138", "139", "150", "151", "152", "157", "158", "159", "182", "183", "184", "187", "188", "147", "178",
            //    /*联通*/"130", "131", "132", "155", "156", "145", "185", "186", "176", "175",
            //    /*电信*/"133", "153", "180", "181", "189", "177", "173", "149"
            //};
            if (phoneNumber[0].Equals('1') == false)
            {
                return false;
            }
            return true;
        }

        public static bool CheckRealName(string name)
        {
            if (name.Length < 2)
            {
                return false;
            }

            for (int i = 0; i < name.Length; i++)
            {
                Regex reg = new Regex(@"[\u4e00-\u9fa5]");
                if (!reg.IsMatch(name[i].ToString()))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool CheckId(string cardId)
        {
            string Id = cardId;
            if (Id.Length != 18)
            {
                return false;
            }
            long n = 0;
            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;
            }
            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = Id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
            {
                return false;
            }
            return true;//正确
        }

        public static int GetAge(string cardId)
        {
            if (cardId == null || cardId.Length != 18)
            {
                return -1;
            }

            int year = int.Parse(cardId.Substring(6, 4));
            int month = int.Parse(cardId.Substring(10, 2));
            int day = int.Parse(cardId.Substring(12, 2));

            DateTime today = DateTime.Now;
            int age = today.Year - year;
            //再考虑月、天的因素
            if (today.Month < month || (today.Month == month && today.Day < day))
            {
                age--;
            }
            return age;
        }

        //获取玩家认证状态
        public static UserAccount.AgeLevel GetAgeLevel(int age)
        {
            if (age == -1)
            {
                return UserAccount.AgeLevel.Unknown;
            }
            if (age < 8)
            {
                return UserAccount.AgeLevel.Under8;
            }
            else if (age < 16)
            {
                return UserAccount.AgeLevel.Under16;
            }
            else if (age < 18)
            {
                return UserAccount.AgeLevel.Under18;
            }
            else
            {
                return UserAccount.AgeLevel.Adult;
            }
        }
    }
}
