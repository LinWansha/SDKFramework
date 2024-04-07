using System.Collections.Generic;
using SDKFramework.Account.DataSrc;
using SDKFramework.Message;
using SDKFramework.UI;
using UnityEngine;
using UnityEngine.UI;

public class OnClickLoginMediator : UIMediator<OnClickLoginView>
{
    protected UserAccount m_selectedAccount;

    protected override void OnInit()
    {
        base.OnInit();
        View.btnClose.onClick.AddListener(Close);
        View.btnLoginGame.onClick.AddListener(() => { });
        View.btnToLoginUI.onClick.AddListener(() => HabbyFramework.UI.OpenUI(UIViewID.LoginUI));
        View.btnAddAccount.onClick.AddListener(() => HabbyFramework.UI.OpenUI(UIViewID.LoginUI));

        HabbyFramework.Account.AccountHistory.channelList = InitializeChannelData();
        m_selectedAccount = HabbyFramework.Account.CurrentAccount;
        View.currentAccountItem.Setup(m_selectedAccount.UID, m_selectedAccount.LoginChannel);
        PopulateList();

    }

    Dictionary<string, Dictionary<string, UserAccount>> InitializeChannelData() //Mock
    {
        var channelList = new Dictionary<string, Dictionary<string, UserAccount>>();
        channelList["qq"] = new Dictionary<string, UserAccount>
        {
            { "1", new UserAccount() { UID = "1", LoginChannel = UserAccount.ChannelQQ } },
        };
        channelList["weixin"] = new Dictionary<string, UserAccount>
        {
            { "2", new UserAccount() { UID = "2", LoginChannel = UserAccount.ChannelWeiXin } },
        };
        channelList["phone"] = new Dictionary<string, UserAccount>
        {
            { "3", new UserAccount() { UID = "3", LoginChannel = UserAccount.ChannelPhone } },
        };
        channelList["appleid"] = new Dictionary<string, UserAccount>
        {
            { "4", new UserAccount() { UID = "4", LoginChannel = UserAccount.ChannelAppleId } },
        };
        return channelList;
    }

    private List<GameObject> itemList = new List<GameObject>();
    List<AccountItemUI> accountItemUis = new List<AccountItemUI>();

    private GameObject GetItemFromPool()
    {
        foreach (var item in itemList)
        {
            if (!item.activeSelf)
            {
                return item;
            }
        }

        var newItem = ScrollViewExt.Instance.AddNewItem();
        itemList.Add(newItem);
        accountItemUis.Add(newItem.GetComponent<AccountItemUI>());

        return newItem;
    }

    private void ReturnAllItemsToPool()
    {
        accountItemUis.Clear();
        foreach (var item in itemList)
        {
            item.SetActive(false);
        }
    }

    void PopulateList()
    {
        ReturnAllItemsToPool();

        foreach (KeyValuePair<string, Dictionary<string, UserAccount>> channelEntry in HabbyFramework.Account.AccountHistory.channelList)
        {
            foreach (KeyValuePair<string, UserAccount> accountEntry in channelEntry.Value)
            {
                GameObject accountItem = GetItemFromPool();
                
                accountItem.SetActive(true); 
                var accountItemScript = accountItem.GetComponent<AccountItemUI>();
                accountItemScript.Setup(accountEntry.Key, channelEntry.Key);
                
                Button button = accountItem.GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                    m_selectedAccount = accountEntry.Value;
                    OnAccountSelected();
                });
            }
        }
        
        HabbyFramework.Message.Post(new MsgType.RefreshAccountItem(accountItemUis));
    }


    void OnAccountSelected()
    {
        View.ToggleScrollView();
        View.currentAccountItem.Setup(m_selectedAccount.UID, m_selectedAccount.LoginChannel);
    }
}