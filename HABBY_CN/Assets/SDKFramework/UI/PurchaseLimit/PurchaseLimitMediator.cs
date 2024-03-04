using System.Collections.Generic;
using Habby.CNUser;
using SDKFramework.UI;

public class PurchaseLimitMediator : UIMediator<PurchaseLimitView>
{
    private Dictionary<(UserAccount.AgeLevel, LimitType), string> _messageMap;
    protected override void OnInit()
    {
        base.OnInit();
        _messageMap = new Dictionary<(UserAccount.AgeLevel, LimitType), string>
        {
            { (UserAccount.AgeLevel.Unknown, LimitType.Single), AntiAddictionDisaplayText.PurchaseNotice_NotLogin },
            { (UserAccount.AgeLevel.Under8, LimitType.Single), AntiAddictionDisaplayText.PurchaseNotice_SpendUnder8 },
            { (UserAccount.AgeLevel.Unknown, LimitType.Monthly), AntiAddictionDisaplayText.PurchaseNotice_NotLogin },
            { (UserAccount.AgeLevel.Under8, LimitType.Monthly), AntiAddictionDisaplayText.PurchaseNotice_SpendUnder8 },

            { (UserAccount.AgeLevel.Under16, LimitType.Single), AntiAddictionDisaplayText.PurchaseNotice_SpendUnder16 },
            { (UserAccount.AgeLevel.Under18, LimitType.Single), AntiAddictionDisaplayText.PurchaseNotice_SpendUnder18 },
            { (UserAccount.AgeLevel.Under16, LimitType.Monthly), AntiAddictionDisaplayText.PurchaseNotice_MonthlySpendUnder16 },
            { (UserAccount.AgeLevel.Under18, LimitType.Monthly), AntiAddictionDisaplayText.PurchaseNotice_MonthlySpendUnder18 }
        };
    }

    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
        view.btnSure.onClick.AddListener(Close);

        UserAccount account = AccountManager.Instance.CurrentAccount;
        if (account.AgeRange == UserAccount.AgeLevel.Adult)
        {
            Close();
            return;
        }
        LimitType type = (LimitType)arg;
        SetPurchaseLimitNotice(account, type);
    }

    private void SetPurchaseLimitNotice(UserAccount account, LimitType type)
    {
        view.detail.text = _messageMap[(account.AgeRange, type)];
    }
    
    protected override void OnHide()
    {
        view.btnSure.onClick.RemoveListener(Close);
        base.OnHide();
    }
}

public enum LimitType : byte
{
    Single,
    Monthly
}