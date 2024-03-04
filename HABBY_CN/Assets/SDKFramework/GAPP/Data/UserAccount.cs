using System;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

namespace Habby.CNUser
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
        
        [SerializeField]
        string accessToken;
        public string AccessToken { get { return accessToken; } set { accessToken = value; } }

        [SerializeField]
        string refreshToken;
        public string RefreshToken { get { return refreshToken; } set { refreshToken = value; } }

        // [SerializeField]
        // long expireTime;
        // public long ExpireAt { get { return expireTime; } set { expireTime = value; } }
        
        [SerializeField]
        string unionId;
        public string UnionId { get { return unionId; } set { unionId = value; } }
        
        
        [SerializeField]
        string uid;
        public string UID { get { return uid; } set { uid = value; } }
    
        [FormerlySerializedAs("offlinetime")] [SerializeField]
        long loginTime;
        public long LoginTime { get { return loginTime; } set { loginTime = value; } }
        
        [SerializeField]
        string nickName;
        public string NickName { get { return nickName; } set { nickName = value; } }

        [SerializeField]
        UserLoginState loginState;
        public UserLoginState LoginState { get { return loginState; } set { loginState = value; } }

        [SerializeField]
        GameLevel level;
        public GameLevel Level { get { return level; } set { level = value; } }

        [SerializeField]
        string phoneNumber;
        public string PhoneNumber { get { return phoneNumber; } set { phoneNumber = value; } }
        [SerializeField]
        string password;
        public string Password { get { return password; } set { password = value; } }

        [SerializeField]
        string realName;
        public string RealName { get { return realName; } set { realName = value; } }

        [SerializeField]
        string idcard;
        public string IdCard { get { return idcard; } set { idcard = value; } }

        [SerializeField]
        string channel;
        public string LoginChannel { get { return channel; } set { channel = value; } }
        
        /// <summary>
        /// 登陆方式名：例如短信验证，手机一键登录，qq，微信等，
        /// 注：多个登陆方式可能对应一个LoginChannel（ 例如手机短信，手机一键登录）
        /// </summary>
        [SerializeField]
        string loginMethodName;

        /// <summary>
        /// 登陆方式名：例如短信验证，手机一键登录，qq，微信等，
        /// 注：多个登陆方式可能对应一个LoginChannel（ 例如手机短信，手机一键登录）
        /// </summary>
        public string LoginMethodName
        {
            get { return loginMethodName; } 
            set { loginMethodName = value; }
        }
        [SerializeField]
        string loginServerName;
        
        /// <summary>
        /// 登陆方服务器
        /// </summary>
        public string LoginServerName
        {
            get { return loginMethodName; } 
            set { loginMethodName = value; }
        }
        

        [SerializeField]
        AgeLevel ageLevel;
        public AgeLevel AgeRange { get { return ageLevel; } set { ageLevel = value; } }

        private UserGachaData userGacha;
        public UserGachaData Gacha { get { return userGacha; } set { userGacha = value; } }

        private UserExpenseData userExpense;
        public UserExpenseData IAP { get { return userExpense; } set { userExpense = value; } }

        private UserOnlineData userOnlineData;
        public UserOnlineData Online { get { return userOnlineData; } set { userOnlineData = value; } }
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
            if (gacha != null) userGacha = JsonUtility.FromJson<UserGachaData>(gacha);
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
            if (userGacha != null) info.AddValue("gacha", JsonUtility.ToJson(userGacha, true));
        }

        public void AddGacha(string gacha, int count)
        {
            if (userGacha == null) userGacha = new UserGachaData();
            userGacha.Add(gacha, count);
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

        // public bool IsExpired
        // {
        //     get
        //     {
        //         HLogger.LogWarnFormat("--- IsExpired isExpire: ExpireAt" + ExpireAt + " nowTick=" +  TimerHelper.GetNowTime().Ticks );
        //         if (ExpireAt > 0)
        //         {
        //             return TimerHelper.GetNowTime().Ticks >= ExpireAt;
        //         }
        //         return true;
        //     }
        // }

        public enum GameLevel : int
        {
            Starter = 0, // 新手
            Low_Level = 1,//低等级
            Middle_Level = 2,//中级
            High_Level = 3,//高级
            Master = 4,//大师
            Almighty = 5,//全能的
            God = 6//神
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
        public enum UserLoginState : int
        {
            Logout = 0,
            Logedin = 1,
        }

        public enum AgeLevel : int
        {
            Unknown = 0,  // 未实名
            Under8 = 1,   // 8岁以下
            Under16 = 2,  // 8 - 16岁以下
            Under18 = 3,   // 16 - 18岁
            Adult = 4    // 成年
        }

        public static AgeLevel ParseAgeLevel(int age)
        {
            if (age < 8) return AgeLevel.Under8;
            if (age < 16) return AgeLevel.Under16;
            if (age < 18) return AgeLevel.Under18;
            return AgeLevel.Adult;
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
