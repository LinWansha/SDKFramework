﻿using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SDKFramework.Account.DataSrc
{
    public static class FileSaveLoad
    {
        private static readonly string kUserAccount = Application.persistentDataPath + "/user";
        private static readonly string kUserAccountHistory = Application.persistentDataPath + "/userHistory";


        public static bool HasAccountHistory => File.Exists(kUserAccountHistory);

        public static void SaveHistory(UserAccountHistory accountHistory)
        {
            if (accountHistory == null) {
                Delete(kUserAccountHistory);
                return;
            }

            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                using FileStream file = File.Create(kUserAccountHistory);
                bf.Serialize(file, accountHistory);
            }
            catch (Exception e)
            {
                Log.Error(e);
                DeleteHistory();
            }
        }

        public static void DeleteHistory()
        {
            Delete(kUserAccountHistory);
        }
        
        public static UserAccountHistory LoadHistory()
        {
            UserAccountHistory user = null;
            if (HasSave(kUserAccountHistory)) {
                BinaryFormatter bf = new BinaryFormatter();
                try
                {
                    using FileStream file = File.Open(kUserAccountHistory, FileMode.Open);
                    user = (UserAccountHistory) bf.Deserialize(file);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }

            return user;
        }
        
        
        
        public static void SaveAccount(UserAccount account)
        {
            if (account == null) {
                Delete(kUserAccount);
                return;
            }

           
            try
            {
                using FileStream file = File.Create(kUserAccount);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(file, account);
            }
            catch
            {
                Delete(kUserAccount);
                DeleteHistory();
            }
        }

        public static UserAccount LoadAccount()
        {
            UserAccount user = null;
            if (HasSave(kUserAccount)) {
                BinaryFormatter bf = new BinaryFormatter();
                try
                {
                    using FileStream file = File.Open(kUserAccount, FileMode.Open);
                    user = (UserAccount) bf.Deserialize(file);
                }
                catch (Exception e)
                {
                    Delete(kUserAccount);
                    DeleteHistory();
                    Log.Error(e);

                }
            }

            return user;
        }

        private static void SaveDataToJson(object data, string path)
        {
            if (data == null) {
                if (path != null) Delete(path);
                return;
            }
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json, Encoding.UTF8);
        }

        private static async Task<string> LoadJsonFromFile(string path)
        {
            if (!File.Exists(path)) return null;

            using StreamReader sr = new StreamReader(path);
            return await sr.ReadToEndAsync();
        }

        private static bool HasSave(string filepath)
        {
            return File.Exists(filepath);
        }

        private static void Delete(string filepath)
        {
            if (File.Exists(filepath)) {
                File.Delete(filepath);
            }
        }
    }
}
