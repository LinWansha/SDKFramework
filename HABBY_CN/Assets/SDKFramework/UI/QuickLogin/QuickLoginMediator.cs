using System.Collections.Generic;
using SDKFramework;
using SDKFramework.Account;
using SDKFramework.Account.Net;
using SDKFramework.Message;
using SDKFramework.UI;
using SDKFramework.Utils;
using UnityEngine;
using UnityEngine.UI;

public class QuickLoginMediator : UIMediator<QuickLoginView>
{

    private AccountModule AccountModule;

    protected override void OnInit()
    {
        base.OnInit();

        View.btnClose.onClick.AddListener(Close);
        
        View.btnLoginGame.onClick.AddListener(() =>
        {
            HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.click_login_bt);
            // AccountModule.loginRunner.Execute(AccountModule.LoginMethodMap[Global.Channel]);
            HabbyUserClient.Instance.LoginWithToken((response) =>
            {
                switch (response.code)
                {
                    case Response.CODE_SUCCESS:
                        HabbyFramework.Account.RealNameLogin((success) =>
                        {
                            AccountLog.Info(success ? "RealNameLogin Success" : "RealNameLogin Failed");
                        });
                        break;
                    case Response.CODE_APP_TOKEN_EXPIRE:
                        HabbyTextHelper.Instance.ShowTip($"{Global.Channel}  授权过期,请重新授权");
                        break;
                    case Response.CAPTCHA_INVALID:
                        break;
                }
            },Global.Channel,AccountModule.CurrentAccount.AccessToken);
        });
        
        View.btnToLoginUI.onClick.AddListener(() =>
        {
            HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.click_otherchannel);
            HabbyFramework.UI.OpenUISingle(UIViewID.LoginUI);
        });
        View.btnAddAccount.onClick.AddListener(() => HabbyFramework.UI.OpenUISingle(UIViewID.LoginUI));
    }

    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
        
        HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.login_button_show);
        AccountModule = HabbyFramework.Account;
        ResetUIState();
        PopulateList();
    }

    private void ResetUIState()
    {
        View.btnLoginGame.interactable = true;
        View.btnAddAccount.gameObject.SetActive(true);
        View.btnToDelete.gameObject.SetActive(true);
        View.btnDone.gameObject.SetActive(false);
        View.currentAccountItem.Setup(AccountModule.CurrentAccount.NickName, AccountModule.CurrentAccount.LoginChannel);
    }
    
    List<GameObject> activeItems = new List<GameObject>();
    List<AccountItemUI> accountItemUis = new List<AccountItemUI>();

    private GameObject CreateNewItem()
    {
        var newItem = View.dropDown.AddNewItem();
        return newItem;
    }

    private void DestroyAllItems()
    {
        foreach (var item in activeItems)
        {
            View.dropDown.RemoveItem(item);
        }
        activeItems.Clear();
        accountItemUis.Clear();
    }

    void PopulateList()
    {
        DestroyAllItems();

        foreach (var channelEntry in AccountModule.AccountHistory.channelList)
        {
            foreach (var accountEntry in channelEntry.Value)
            {
                GameObject accountItem = CreateNewItem();

                accountItem.SetActive(true);
                var accountItemScript = accountItem.GetComponent<AccountItemUI>();
                accountItemScript.Setup(accountEntry.Value.NickName, channelEntry.Key);

                var selectedAccount = accountEntry.Value;

                Button button = accountItem.GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                    AccountModule.SetCurrentAccount(selectedAccount);
                    View.ToggleScrollView();
                    View.currentAccountItem.Setup(selectedAccount.NickName, selectedAccount.LoginChannel);
                });

				accountItemScript.btnDelete.onClick.RemoveAllListeners();
                accountItemScript.btnDelete.onClick.AddListener(() =>
                {
                    View.dropDown.RemoveItem(accountItem);
                    HabbyFramework.Account.AccountHistory.DeleteById(selectedAccount.UID, selectedAccount.LoginChannel);
                    activeItems.Remove(accountItem);
                    if (View.dropDown.ChildrenCount <= 1)
                    {
                        Close();
                        HabbyFramework.Account.ClearCurrent();
                        HabbyFramework.Message.Post(new MsgType.ResetPrivacyToggle());
                    }
                });

                activeItems.Add(accountItem);
                accountItemUis.Add(accountItemScript);
            }
        }

        HabbyFramework.Message.Post(new MsgType.RefreshAccountItem(accountItemUis));
    }

    protected override void OnHide()
    {
        View.ToggleScrollView(true);
        base.OnHide();
    }
}