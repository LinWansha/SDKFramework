using System.Collections.Generic;
using Habby.CNUser;
using SDKFramework.UI;

public class PurchaseLimitMediator : UIMediator<PurchaseLimitView>
{
    protected override void OnInit(PurchaseLimitView view)
    {
        base.OnInit(view);
        view.btnSure.onClick.AddListener(Close);
    }

    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
        
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
        var messageMap = new Dictionary<(UserAccount.AgeLevel, LimitType), string>
        {
            { (UserAccount.AgeLevel.Unknown, LimitType.Single), AntiAddictionDisaplayText.PurchaseNotice_SpendUnder16 },
            { (UserAccount.AgeLevel.Under8, LimitType.Single), AntiAddictionDisaplayText.PurchaseNotice_SpendUnder16 },
            { (UserAccount.AgeLevel.Unknown, LimitType.Monthly), AntiAddictionDisaplayText.PurchaseNotice_MonthlySpendUnder16 },
            { (UserAccount.AgeLevel.Under8, LimitType.Monthly), AntiAddictionDisaplayText.PurchaseNotice_MonthlySpendUnder16 },

            { (UserAccount.AgeLevel.Under16, LimitType.Single), AntiAddictionDisaplayText.PurchaseNotice_SpendUnder18 },
            { (UserAccount.AgeLevel.Under18, LimitType.Single), AntiAddictionDisaplayText.PurchaseNotice_SpendUnder18 },
            { (UserAccount.AgeLevel.Under16, LimitType.Monthly), AntiAddictionDisaplayText.PurchaseNotice_MonthlySpendUnder18 },
            { (UserAccount.AgeLevel.Under18, LimitType.Monthly), AntiAddictionDisaplayText.PurchaseNotice_MonthlySpendUnder18 }
        };

        view.detail.text = messageMap[(UserAccount.AgeLevel.Under18, type)];
    }
}

public enum LimitType : byte
{
    Single,
    Monthly
}