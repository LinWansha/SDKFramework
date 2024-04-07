using System;
using System.Collections.Generic;

namespace SDKFramework.Account.DataSrc
{
    [Serializable]
    public class WebviewUserData
    {
        public string name;
        public string id;
        public string type;
        public long? logintime;

        public WebviewUserData(UserAccount account)
        {
            name = account.NickName;
            id = account.UID;
            logintime = account.LoginTime;
            type = account.LoginChannel;
            if (logintime == null || logintime == 0)
            {
                logintime = DateTime.Now.ToFileTimeUtc();
            }
        }

        
    }
    
    [Serializable]
    public class UserAccountHistory
    {
        public Dictionary<string, Dictionary<string,UserAccount>> channelList = new Dictionary<string, Dictionary<string, UserAccount>>();

        public UserAccount GetUserAccount(string channelName, string uid)
        {
            if (channelList != null && channelList.ContainsKey(channelName) && channelList[channelName].ContainsKey(uid))
            {
                return channelList[channelName][uid];
            }

            return null;
        }

        public bool HasAccountHistory => channelList.Count > 0;

        public Dictionary<string,UserAccount> GetChannelHistory(string channelName)
        {
            channelList ??= new Dictionary<string, Dictionary<string, UserAccount>>();

            if (!channelList.ContainsKey(channelName))
            {
                return new Dictionary<string,UserAccount>();
            }
            return channelList[channelName];
        }

        public Dictionary<string, List<WebviewUserData>> GetWebviewData()
        {
            if (channelList == null)
            {
                channelList = new Dictionary<string, Dictionary<string, UserAccount>>();
            }
           
            
            // Dictionary<string, Object> result = new Dictionary<string, Object>();
            Dictionary<string, List<WebviewUserData>> result = new Dictionary<string, List<WebviewUserData>>();
            // foreach (KeyValuePair<string,Dictionary<string,UserAccount>> keyValuePair in channelList)
            foreach (var item in this.channelList)
            {
                if (!result.ContainsKey(item.Key))
                {
                    result[item.Key] = new List<WebviewUserData>();
                }

                foreach (KeyValuePair<string,UserAccount> valuePair in item.Value)
                {
                    result[item.Key].Add(new WebviewUserData(valuePair.Value));
                }

                if (result[item.Key].Count > 1)
                {
                    result[item.Key].Sort((item1, item2) => { return item1.logintime > item2.logintime ? 1 : -1;});
                }
            }

            if (!result.ContainsKey(UserAccount.ChannelPhone))
            {
                result[UserAccount.ChannelPhone] = new List<WebviewUserData>();
            }

            if (!result.ContainsKey(UserAccount.ChannelWeiXin))
            {
                result[UserAccount.ChannelWeiXin] = new List<WebviewUserData>();
            }
            
            if (!result.ContainsKey(UserAccount.ChannelAppleId))
            {
                result[UserAccount.ChannelAppleId] = new List<WebviewUserData>();
            }
            
            if (!result.ContainsKey(UserAccount.ChannelQQ))
            {
                result[UserAccount.ChannelQQ] = new List<WebviewUserData>();
            }
            //ChannelPhoneQuick 和phone同属手机
            return result;
        }

        public void SaveAccount(UserAccount account)
        {
            if (account == null)
            {
                return;
            }
            channelList ??= new Dictionary<string, Dictionary<string, UserAccount>>();

            if (!channelList.ContainsKey(account.LoginChannel))
            {
                channelList[account.LoginChannel] = new Dictionary<string,UserAccount>();
            }

            channelList[account.LoginChannel][account.UID] = account;
            Save();
        }

        public void Delete(UserAccount account)
        {
            if (channelList == null || account == null || !channelList.ContainsKey(account.LoginChannel) || !channelList[account.LoginChannel].ContainsKey(account.UID))
            {
                return;
            }
            channelList[account.LoginChannel].Remove(account.UID);
            Save();
        }
        
        public void DeleteById(string uid,string channel)
        {
            if (channelList == null  || !channelList.ContainsKey(channel) || !channelList[channel].ContainsKey(uid))
            {
                return;
            }
            channelList[channel].Remove(uid);
            Save();
        }
        

        public void Save()
        {
            FileSaveLoad.DeleteHistory();
            FileSaveLoad.SaveHistory(this);
        }

        public void DeleteHistory()
        {
            if (channelList != null)
            {
                channelList.Clear();
            }
            FileSaveLoad.DeleteHistory();
            Save();
        }
    }
}