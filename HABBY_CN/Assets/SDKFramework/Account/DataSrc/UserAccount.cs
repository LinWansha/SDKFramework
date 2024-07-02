using System;
using System.Runtime.Serialization;
using SDKFramework.Account.AntiAddiction;
using UnityEngine;
using UnityEngine.Serialization;

namespace SDKFramework.Account.DataSrc
{
    [Serializable]
    public class UserAccount
    {
        #region channel name
        public const string ChannelTapTap = "taptap";
        public const string ChannelWeiXin = "weixin";
        public const string ChannelQQ = "qq";
        public const string ChannelPhoneQuick = "phonequick";
        public const string ChannelGameCenter = "gc";
        public const string ChannelAppleId = "appleid";
        public const string ChannelPhone = "phone";
        public const string ChannelAccount = "account";
        public const string ChannelNACA = "ncac";
        public const string ChannelEditor = "editor";
        
        public const string ChannelTraditional = "traditional";
        #endregion

        private bool isLogin;
        public bool IsLogin { get => isLogin; set => isLogin = value; }
        
        bool isAgreePrivacy;
        public bool IsAgreePrivacy{ get => isAgreePrivacy; set => isAgreePrivacy = value; }
        
        string accessToken;
        public string AccessToken{ get => accessToken; set => accessToken = value; }

        
        string refreshToken;
        public string RefreshToken { get => refreshToken; set => refreshToken = value; }
        
        
        string unionId;
        public string UnionId{ get => unionId; set => unionId = value; }
        
        
        string uid;
        public string UID{ get => uid; set => uid = value; }
    

        string nickName;
        public string NickName{ get => nickName; set => nickName = value; }
        
        string phoneNumber;
        public string PhoneNumber  { get => phoneNumber; set => phoneNumber = value; }
        
        
        string realName;
        public string RealName { get => realName; set => realName = value; }
        
        string idcard;
        public string IdCard  { get => idcard; set => idcard = value; }

        
        string channel;
        public string LoginChannel { get => channel; set => channel = value; }
       
        
        AgeLevel ageLevel;
        public AgeLevel AgeRange { get => ageLevel; set => ageLevel = value;  }

        
        private UserExpenseData userExpense;

        public UserExpenseData IAP
        {
            get
            {
                if (userExpense == null)
                {
                    ResetExpense(0, 0, 0);
                    AccountLog.Info("IAP数据尚未刷新");
                }

                return userExpense;
            }
            set => userExpense = value;
        }

        
        private UserOnlineData userOnlineData;
        public UserOnlineData Online  { get => userOnlineData; set =>userOnlineData = value; }

        
        private int age;
        public int Age { get => age; set => age = value; }
        
        private bool isNewUser;
        public bool IsNewUser { get => isNewUser; set => isNewUser = value; }

        public UserAccount() { }

        public void AddIap(double money)
        {
            userExpense ??= new UserExpenseData();
            userExpense.Add(money);
        }
        public void AddOnline(int seconds)
        {
            userOnlineData ??= new UserOnlineData();
            userOnlineData.Add(seconds);
        }

        public void ResetExpense(long total, long monthly, long today)
        {
            userExpense ??= new UserExpenseData();
            userExpense.Refresh();
            userExpense.totalExpense.value = total;
            userExpense.monthlyExpense.value = monthly;
            userExpense.dailyExpense.value = today;
        }

        public void ResetOnline(int totalSeconds, int todaySeconds)
        {
            userOnlineData ??= new UserOnlineData();
            userOnlineData.Refresh();

            userOnlineData.Total = totalSeconds;
            userOnlineData.Today = todaySeconds;
        }

        public void RefreshMonthlyExpense(double value)
        { 
            userExpense ??= new UserExpenseData();
            userExpense.Refresh();
            userExpense.monthlyExpense.value = value;
        }
        
        public void RefreshTotalExpense(double value)
        { 
            userExpense ??= new UserExpenseData();
            userExpense.Refresh();
            userExpense.totalExpense.value = value;
        }
        
        public enum AgeLevel : int
        {
            Unknown = 0,    // 未实名
            Under8 = 1,     // 8岁以下
            Under16 = 2,    // 8 - 16岁以下
            Under18 = 3,    // 16 - 18岁
            Adult = 4       // 成年
        }

        public override string ToString()
        {
            return string.Format("id={0} name={1} LoginChannel={2} ageLevel={3} token={4}", UID, nickName, LoginChannel,
                ageLevel, accessToken);
        }
    }
}
