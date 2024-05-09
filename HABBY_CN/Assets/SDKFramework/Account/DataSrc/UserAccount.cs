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
        
        public const string ChannelTraditional = "traditional";
        #endregion
        
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
    
        [FormerlySerializedAs("offlinetime")] 
        long loginTime;
        public long LoginTime { get => loginTime; set => loginTime = value; }
        
        
        string nickName;
        public string NickName{ get => nickName; set => nickName = value; }
        
        string phoneNumber;
        public string PhoneNumber  { get => phoneNumber; set => phoneNumber = value; }

        private string userId;
        public string UserId{ get => userId; set => userId = value; }

        string password;
        public string Password { get => password; set => password = value; }

        
        string realName;
        public string RealName { get => realName; set => realName = value; }

        
        string idcard;
        public string IdCard  { get => idcard; set => idcard = value; }

        
        string channel;
        public string LoginChannel { get => channel; set => channel = value; }
        
        /// <summary>
        /// 登陆方式名：例如短信验证，手机一键登录，qq，微信等，
        /// 注：多个登陆方式可能对应一个LoginChannel（ 例如手机短信，手机一键登录）
        /// </summary>
        
        string loginMethodName;
        /// <summary>
        /// 登陆方式名：例如短信验证，手机一键登录，qq，微信等，
        /// 注：多个登陆方式可能对应一个LoginChannel（ 例如手机短信，手机一键登录）
        /// </summary>
        public string LoginMethodName{ get => loginMethodName; set => loginMethodName = value; }
        
        string loginServerName;
        /// <summary>
        /// 登陆方服务器
        /// </summary>
        public string LoginServerName { get => loginServerName; set => loginServerName = value; }
       
        AgeLevel ageLevel;
        public AgeLevel AgeRange { get => ageLevel; set => ageLevel = value;  }

        private UserExpenseData userExpense;
        public UserExpenseData IAP { get => userExpense;  set =>userExpense = value; }

        private UserOnlineData userOnlineData;
        public UserOnlineData Online  { get => userOnlineData; set =>userOnlineData = value; }

        private int age;
        public int Age { get => age; set => age = value; }


        public UserAccount() { }

        public UserAccount(SerializationInfo info, StreamingContext context)
        {
            accessToken = info.GetString("access");
            refreshToken = info.GetString("refresh");
            uid = info.GetString("uid");
            nickName = info.GetString("nick");
            // expireTime = info.GetInt64("expire");
            unionId = info.GetString("unionId");
            phoneNumber = info.GetString("ph");
            password = info.GetString("pa");

            realName = info.GetString("n");
            idcard = info.GetString("i");
            channel = info.GetString("c");
            ageLevel = (AgeLevel)info.GetInt16("age");

            string online = info.GetString("online");
            if (online != null) userOnlineData = JsonUtility.FromJson<UserOnlineData>(online);

            string expense = info.GetString("expense");
            if (expense != null) userExpense = JsonUtility.FromJson<UserExpenseData>(expense);

            string gacha = info.GetString("gacha");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("access", accessToken);
            if (refreshToken != null) info.AddValue("refresh", refreshToken);
            if (uid != null) info.AddValue("uid", uid);
            // info.AddValue("expire", expireTime);
            info.AddValue("unionId", unionId);

            if (phoneNumber != null) info.AddValue("ph", phoneNumber);
            if (password != null) info.AddValue("pa", password);

            if (nickName != null) info.AddValue("nick", nickName);

            if (realName != null) info.AddValue("n", realName);
            if (idcard != null) info.AddValue("i", idcard);
            info.AddValue("c", (string)channel);
            info.AddValue("age", (int)ageLevel);

            if (userOnlineData != null) info.AddValue("online", JsonUtility.ToJson(userOnlineData, true));
            if (userExpense != null) info.AddValue("expense", JsonUtility.ToJson(userExpense, true));
        }

        public void AddIap(double money)
        {
            if (userExpense == null) userExpense = new UserExpenseData();
            userExpense.Add(money);
        }
        public void AddOnline(int seconds)
        {
            if (userOnlineData == null) userOnlineData = new UserOnlineData();
            userOnlineData.Add(seconds);
        }

        public void ResetExpense(long total, long monthly, long today)
        {
            if (userExpense == null) userExpense = new UserExpenseData();
            userExpense.Refresh();
            userExpense.totalExpense.value = total;
            userExpense.monthlyExpense.value = monthly;
            userExpense.dailyExpense.value = today;
        }

        public void ResetOnline(int totalSeconds, int todaySeconds)
        {
            if (userOnlineData == null) userOnlineData = new UserOnlineData();
            userOnlineData.Refresh();

            userOnlineData.Total = totalSeconds;
            userOnlineData.Today = todaySeconds;
        }

        public void RefreshMonthlyExpense(double value)
        { 
            if (userExpense == null) userExpense = new UserExpenseData();
            userExpense.Refresh();
            userExpense.monthlyExpense.value = value;
        }

        public enum UserLoginChannel : int
        {
            NotLogin = 0,
            Visitor = 1,
            HabbyAccount = 2,
            Wechat = 3,
            GameCenter = 4,
            GooglePlay = 5,
            AndroidChannel,//目前暂不具体细分
            Traditional,  // 自建用户名密码形式
            TapTap, // taptap TODO:需要和后端确认
        }
        
        public enum AgeLevel : int
        {
            Unknown = 0,  // 未实名
            Under8 = 1,   // 8岁以下
            Under16 = 2,  // 8 - 16岁以下
            Under18 = 3,   // 16 - 18岁
            Adult = 4    // 成年
        }

        public void SaveLoginTime()
        {
            LoginTime = DateTime.Now.ToFileTimeUtc();
        }

        public override string ToString()
        {
            return string.Format("id={0} name={1} LoginChannel={2} ageLevel={3} token={4}", UID, nickName, LoginChannel,
                ageLevel, accessToken);
        }
    }
}
