using SDKFramework;
using SDKFramework.Account;
using SDKFramework.Account.DataSrc;
using SDKFramework.Account.Net;
using SDKFramework.Account.Utils;
using SDKFramework.UI;
using SDKFramework.Utils;

public class RealNameMediator : UIMediator<RealNameView>
{
    private string Name, IdCard;
    private UserAccount m_Account;

    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
        HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.verify_show);
        
        AccountModule.OnValidateIdentityResult += onResult;
        View.btConfirm.onClick.AddListener(Confirm);
        View.nameInput.onEndEdit.AddListener(OnEditEnd);
        View.idInput.onEndEdit.AddListener(OnEditEnd);

        m_Account = HabbyFramework.Account.CurrentAccount;
        if (m_Account == null)
        {
            Close();
            HabbyFramework.Account.CheckUser();
        }
        else
        {
            View.nameInput.text = m_Account.RealName;
            View.idInput.text = m_Account.IdCard;
        }
    }
    protected override void OnHide()
    {
        View.btConfirm.onClick.RemoveListener(Confirm);
        View.idInput.onEndEdit.RemoveListener(OnEditEnd);
        View.nameInput.onEndEdit.RemoveListener(OnEditEnd);
        AccountModule.OnValidateIdentityResult -= onResult;
        base.OnHide();
    }
    

    private void setNotice(string text)
    {
        View.notice.gameObject.SetActive(true);
        View.notice.text = text;
    }
    
    public void Confirm()
    {
        if (!InputFully()) return;
        HabbyFramework.Analytics.TGA_cn_login(LoginStepCN.verify_submit);
        if (!LocalIdentityUtil.IsChineseName(Name))
        {
            setNotice(LoginErrorConst.IDENTITY_WRONG_NAME);
            return;
        }

        if (!LocalIdentityUtil.IsValidIDCard(IdCard))
        {
            setNotice(LoginErrorConst.IDENTITY_WRONG_ID);
            return;
        }

        if (m_Account == null)
        {
            HabbyFramework.Account.CheckUser();
            return;
        }

        m_Account.RealName =Name;

        string id = IdCard.Trim();
        if (id.Contains("x"))
        {
            id = id.ToUpper();
        }

        m_Account.IdCard = id;
        
        BirthdayAgeSex entity = LocalIdentityUtil.GetBirthdayAgeSex(m_Account.IdCard);
        m_Account.AgeRange = LocalIdentityUtil.ParseAgeLevel(entity.Age);
        m_Account.Age = entity.Age;
        
        HabbyFramework.Account.ValidateIdentity();
        // HabbyFramework.Account.LocalValidateIdentity();
    }

    private void OnEditEnd(string arg0)
    {
        if (View.notice && View.notice.gameObject.activeSelf)
            View.notice.gameObject.SetActive(false);
    }
    private bool InputFully()
    {
        if (!string.IsNullOrEmpty(View.nameInput.text) && !string.IsNullOrEmpty(View.idInput.text))
        {
            Name = View.nameInput.text;
            IdCard = View.idInput.text;
            return true;
        }

        HabbyTextHelper.Instance.ShowTip(LoginErrorConst.IDENTITY_INPUT_ISNULL);
        return false;
    }

    private void onResult(bool isSuccess,int code)
    {
        if (isSuccess)
        {
            Close();
            Log.Info($"Identity successful || channel: {m_Account.LoginChannel} token: {m_Account.AccessToken}");
        }
        else
        {
            Log.Warn($"----- UserIdentifyPopup rps error:code={code}");
            HabbyTextHelper.Instance.ShowTip(string.Format(LoginErrorConst.IDENTITY_FAILURE,code));
            switch (code)
            {
                case IdentityResponse.PARAM_ERROR:
                    setNotice(LoginErrorConst.PARAM_ERROR);
                    break;
                case IdentityResponse.USER_NOT_FOUND:
                    setNotice(LoginErrorConst.USER_NOT_FOUND);
                    break;
                case IdentityResponse.ID_CARD_EXIST:
                    setNotice(LoginErrorConst.ID_CARD_EXIST);
                    break;
                case IdentityResponse.TOKEN_EXPIRE:
                    setNotice(string.Format(LoginErrorConst.OAUTH_EXPIRE,Global.Channel));
                    break;
                case IdentityResponse.SERVER_FATAL_ERROR:
                    setNotice(LoginErrorConst.SERVER_FATAL_ERROR);
                    break;
                case IdentityResponse.SERVER_BUSY:
                    setNotice(LoginErrorConst.SERVER_BUSY);
                    break;
                case IdentityResponse.GAME_SERVER_ERROR:
                    setNotice(LoginErrorConst.GAME_SERVER_ERROR);
                    break;
                case IdentityResponse.ID_CARD_CHECK_PENDING:
                    setNotice(LoginErrorConst.ID_CARD_CHECK_PENDING);
                    break;
                case IdentityResponse.ID_CARD_OVER_COUNT:
                    setNotice(LoginErrorConst.ID_CARD_OVER_COUNT);
                    break;
                case IdentityResponse.ID_CARD_CHECK_FAILED:
                case IdentityResponse.ERROR:
                    setNotice(LoginErrorConst.IDENTITY);
                    break;
                default:
                    setNotice(LoginErrorConst.UN_KNOW);
                    break;
            }
        }
    }
}