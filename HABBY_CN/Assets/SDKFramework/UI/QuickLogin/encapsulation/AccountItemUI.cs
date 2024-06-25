using System;
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
                Log.Error("Channel Name Error");
                return null; 
        }
    }

    public void SwitchBtnDelete()
    {
        if (btnDelete!=null)
        {
            btnDelete.gameObject.SetActive(!btnDelete.gameObject.activeSelf);
        }
    }
}