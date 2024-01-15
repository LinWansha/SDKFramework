using Habby.CNUser;
using SDKFramework.UI;

public class PurchaseLimitMediator : UIMediator<PurchaseLimitView>
{
    protected override void OnInit(PurchaseLimitView view)
    {
        base.OnInit(view);
        view.btnSure.onClick.AddListener(Close);
    }
    
    public void SetRemainMsg(string str)
    {
        view.residueLimit.text = str;
    }
    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
        var account= arg as UserAccount;
        if (account!=null)
        {
            switch (account.AgeRange)
            {
                case UserAccount.AgeLevel.Unknown:
                case UserAccount.AgeLevel.Under8:
                    //OpenOncePurchaseNoticeWithoutRemain(account);
                    break;
                case UserAccount.AgeLevel.Under16:
                case UserAccount.AgeLevel.Under18:
                    //OpenOncePurchaseNoticeWithRemain(account);
                    break;
                default:
                    return;
            }
        }
    }
    // public void OpenMonthlyPurchaseNotice(UserAccount account)
    // {
    //     switch (account.AgeRange) {
    //         case UserAccount.AgeLevel.Under16:
    //             purchaseLimitPopup.SetMsg(ErrPopupType.Usually, AntiAddictionDisaplayText.PurchaseNotice_MonthlySpendUnder16);
    //             purchaseLimitPopup.SetRemainMsg(string.Format(AntiAddictionDisaplayText.MonthRemain, (PurchaseChecker.MonthlyAmountLimit_U16- account.IAP.Monthly)));
    //
    //             break;
    //         case UserAccount.AgeLevel.Under18:
    //             purchaseLimitPopup.SetMsg(ErrPopupType.Usually, AntiAddictionDisaplayText.PurchaseNotice_MonthlySpendUnder18);
    //             purchaseLimitPopup.SetRemainMsg(string.Format(AntiAddictionDisaplayText.MonthRemain, (PurchaseChecker.MonthlyAmountLimit_U18 - account.IAP.Monthly)));
    //
    //             break;
    //         default:
    //             return;
    //     }
    // }
    
    
    // public void OpenOncePurchaseNoticeWithoutRemain(UserAccount account)
    // {
    //
    //     switch (account.AgeRange) {
    //         case UserAccount.AgeLevel.Unknown:
    //             errorMsgPopup.SetMsg(ErrPopupType.Usually, AntiAddictionDisaplayText.PurchaseNotice_NotLogin);
    //             break;
    //         case UserAccount.AgeLevel.Under8:
    //             errorMsgPopup.SetMsg(ErrPopupType.Usually, AntiAddictionDisaplayText.PurchaseNotice_SpendUnder8);
    //             break;
    //         default:
    //             return;
    //     }
    //
    // }
}