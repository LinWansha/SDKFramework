using SDKFramework.Message;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SDKFramework.Utils
{
    public class UGUIEventListener : MonoBehaviour,IPointerClickHandler
    {
        [SerializeField] private UIViewID ViewID;
        public void OnPointerClick(PointerEventData eventData)
        {
            HabbyFramework.Message.Post(new MsgType.ClosePopup(ViewID));
        }
    }
}
