using System;
using System.Collections.Generic;
using SDKFramework.Message;
using SDKFramework.UI;
using UnityEngine;
using UnityEngine.UI;

[UIView(typeof(OnClickLoginMediator), UIViewID.OnClickLoginUI)]
public class OnClickLoginView : UIView
{

    public Transform dirSigns;

    public GameObject scrollView;

    public Button btnLoginGame;

    public Button btnToLoginUI;

    public Button btnClose;

    public AccountItemUI currentAccountItem;

    public Button btnAddAccount;


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
    public void ToggleScrollView()
    {
        bool isActive = scrollView.activeSelf;

        scrollView.SetActive(!isActive);

        float rotationAngle = isActive ? -180f : 180f;
        dirSigns.transform.Rotate(new Vector3(0, 0, rotationAngle));
    }
}