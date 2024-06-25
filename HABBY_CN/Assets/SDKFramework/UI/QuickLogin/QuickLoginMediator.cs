using System.Collections.Generic;
using SDKFramework;
using SDKFramework.Account;
using SDKFramework.Account.DataSrc;
using SDKFramework.Message;
using SDKFramework.UI;
using UnityEngine;
using UnityEngine.UI;

public class QuickLoginMediator : UIMediator<QuickLoginView>
{

    private AccountModule AccountModule;
    
    private Dictionary<string, LoginChannel> OfficialChannelMap;

    protected override void OnInit()
    {
        base.OnInit();

        AccountModule = HabbyFramework.Account;
        OfficialChannelMap = new Dictionary<string, LoginChannel>()
        {
            { UserAccount.ChannelQQ, LoginChannel.QQ },
            { UserAccount.ChannelPhone, LoginChannel.Phone },
            { UserAccount.ChannelWeiXin, LoginChannel.WX },
            { UserAccount.ChannelAppleId, LoginChannel.Apple },
        };
        View.btnClose.onClick.AddListener(Close);
        View.btnLoginGame.onClick.AddListener(() =>
        {
            HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.click_login_bt);
            AccountModule.loginRunner.Execute(OfficialChannelMap[Global.Channel]);
        });
        View.btnToLoginUI.onClick.AddListener(() =>
        {
            HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.click_otherchannel);
            HabbyFramework.UI.OpenUISingle(UIViewID.LoginUI);
        });
        View.btnAddAccount.onClick.AddListener(() => HabbyFramework.UI.OpenUISingle(UIViewID.LoginUI));

        View.currentAccountItem.Setup(AccountModule.CurrentAccount.UID, AccountModule.CurrentAccount.LoginChannel);
        PopulateList();

    }

    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
        HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.login_button_show);
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

        var newItem = View.dropDown.AddNewItem();
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

        foreach (var channelEntry in AccountModule.AccountHistory.channelList)
        {
            foreach (var accountEntry in channelEntry.Value)
            {
                GameObject accountItem = GetItemFromPool();
            
                accountItem.SetActive(true); 
                var accountItemScript = accountItem.GetComponent<AccountItemUI>();
                accountItemScript.Setup(accountEntry.Key, channelEntry.Key);
            
                var selectedAccount = accountEntry.Value;

                Button button = accountItem.GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => OnAccountSelected(selectedAccount));
                
                accountItemScript.btnDelete.onClick.AddListener(() =>
                {
                    View.dropDown.RemoveItem(accountItem);
                    HabbyFramework.Account.AccountHistory.DeleteById(selectedAccount.UID,selectedAccount.LoginChannel);
                });
            }
        }
    
        HabbyFramework.Message.Post(new MsgType.RefreshAccountItem(accountItemUis));
    }

    // private void OnAccountDelete()
    // {
    //     View.dropDown.RemoveItem();
    // }

    void OnAccountSelected(UserAccount selectedAccount)
    {
        AccountModule.Save(selectedAccount);
        View.ToggleScrollView();
        View.currentAccountItem.Setup(AccountModule.CurrentAccount.UID, AccountModule.CurrentAccount.LoginChannel);
    }
}