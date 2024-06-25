using System;
using System.Collections.Generic;

namespace SDKFramework.Account.DataSrc
{
    
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