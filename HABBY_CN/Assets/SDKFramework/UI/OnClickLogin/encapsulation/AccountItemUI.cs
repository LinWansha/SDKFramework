using UnityEngine;
using UnityEngine.UI;

public class AccountItemUI : MonoBehaviour
{
    public Button btnSelect;
    public Text uidText;
    public Image channelIconImage;
    public Button btnDelete;
    
    public void Setup(string uid, string channelName)
    {
        uidText.text = uid;
        channelIconImage.sprite = GetChannelIcon(channelName);
        // btnDelete.onClick.AddListener(() =>
        // {
        //     HabbyFramework.Account.AccountHistory.DeleteById(uid,channelName);
        // });
        //btnSelect.onClick.AddListener();
    }
    
    Sprite GetChannelIcon(string channelName)
    {
        switch (channelName)
        {
            case "qq":
            case "weixin":
            case "phone":
            case "appleid":
                return HabbyFramework.Asset.LoadAssets<Sprite>("Textures/" + channelName);
            default:
                return null; // 或者返回一个默认的图标
        }
    }

    public void SwitchBtnDelete()
    {
        btnDelete.gameObject.SetActive(!btnDelete.gameObject.activeSelf);
    }
}