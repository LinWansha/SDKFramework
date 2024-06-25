using System.Collections.Generic;

namespace SDKFramework.Message
{
    public class MsgType
    {
        public struct  ClosePopup
        {
            public UIViewID ViewID;

            public ClosePopup(UIViewID viewId)
            {
                ViewID = viewId;
            }
        }
        
        public struct  RefreshAccountItem
        {
            public List<AccountItemUI> AccountItemUis;
            
            public RefreshAccountItem(List<AccountItemUI>  accountItemUis)
            {
                AccountItemUis = accountItemUis;
            }
        }
        
       
    }

    public class SDKEvent
    {
        public struct ShowNoAgreePrivacyNotice { }
        
        public struct SDKLoginFinish
        {
            public int code;
            public string msg;
            public string uid;
            public bool isNew;
        }
        
        public struct AccountLogout
        {

        }
    }
}