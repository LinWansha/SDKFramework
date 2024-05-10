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
    }
}