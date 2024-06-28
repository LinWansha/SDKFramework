using System;
using System.Collections.Generic;
using SDKFramework.Message;
using SDKFramework.UI;
using UnityEngine;
using UnityEngine.UI;

[UIView(typeof(QuickLoginMediator), UIViewID.QuickLoginUI)]
public class QuickLoginView : UIView
{

    public Transform dirSigns;

    public GameObject scrollView;

    public Button btnLoginGame;

    public Button btnToLoginUI;

    public Button btnClose;

    public AccountItemUI currentAccountItem;

    public Button btnAddAccount;
    public Button btnToDelete;
    public Button btnDone;
    public ScrollViewExt dropDown;


    private void Awake()
    {
        HabbyFramework.Message.Subscribe<MsgType.RefreshAccountItem>(RefreshItemData);
    }

    private List<AccountItemUI> accountItems;
    private void RefreshItemData(MsgType.RefreshAccountItem arg)
    {
        accountItems = arg.AccountItemUis;
    }

    public void SwitchAllItemDelete()
    {
        foreach (var accountItem in accountItems)
        {
            accountItem.SwitchBtnDelete();
        }
    }
    public void ToggleScrollView(bool reset=false)
    {
        if (reset)
        {
            scrollView.SetActive(false);
            dirSigns.transform.Rotate(new Vector3(0, 0, 180));
            return;
        }
        bool isActive = scrollView.activeSelf;

        scrollView.SetActive(!isActive);
        btnLoginGame.interactable = isActive;

        float rotationAngle = isActive ? -180f : 180f;
        dirSigns.transform.Rotate(new Vector3(0, 0, rotationAngle));
    }
}