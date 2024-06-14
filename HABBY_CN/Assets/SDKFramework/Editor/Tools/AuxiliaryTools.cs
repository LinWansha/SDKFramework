using System.Diagnostics;
using SDKFramework.Account.DataSrc;
using UnityEditor;
using UnityEngine;

namespace SDKFramework.Editor.Tools
{
    public class AuxiliaryTools
    {
        [MenuItem("SDKFramework/Open PersistentDataPath")]
        public static void OpenDirectory()
        {
            string directoryPath = Application.persistentDataPath;
        
            try
            {
                Process.Start(directoryPath);
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError("Error opening directory: " + ex.Message);
            }
        }
    
        [MenuItem("SDKFramework/清除本地账号数据")]
        public static void ClearAccountData()
        {
            FileSaveLoad.DeleteHistory();
            FileSaveLoad.SaveAccount(null);
            Log.Info("清除本地账号数据！");
        }
    }
}