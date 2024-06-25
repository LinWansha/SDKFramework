using SDKFramework;
using SDKFramework.Account;
using SDKFramework.Account.DataSrc;
using SDKFramework.Account.Net;
using SDKFramework.Account.Utils;
using SDKFramework.UI;
using SDKFramework.Utils;

public class RealNameMediator : UIMediator<RealNameView>
{
    public const string WrongName = "您输入的姓名有误,请重新输入";
    public const string WrongId = "您输入的身份证号码有误,请重新输入";

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
            setNotice(WrongName);
            return;
        }

        if (!LocalIdentityUtil.IsValidIDCard(IdCard))
        {
            setNotice(WrongId);
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

        HabbyTextHelper.Instance.ShowTip("输入的姓名和身份证号不能为空");
        return false;
    }

    private void onResult(bool isSuccess,int code)
    {
        if (isSuccess)
        {
            Close();
            Log.Info($"实名认证成功 || channel: {m_Account.LoginChannel} token: {m_Account.AccessToken}");
        }
        else
        {
            Log.Warn($"----- UserIdentifyPopup rps error:code={code}");
            HabbyTextHelper.Instance.ShowTip("实名认证失败!错误代码：" + code);
            switch (code)
            {
                case IdentityResponse.PARAM_ERROR:
                    setNotice("输入参数错误");
                    break;
                case IdentityResponse.USER_NOT_FOUND:
                    setNotice("找不到此用户");
                    break;
                case IdentityResponse.ID_CARD_EXIST:
                    setNotice("此身份证已经绑定过其他账号");
                    break;
                case IdentityResponse.TOKEN_EXPIRE:
                    setNotice("登陆已过期,请从新登陆");
                    break;
                case IdentityResponse.SERVER_FATAL_ERROR:
                    setNotice("GM 服务器故障");
                    break;
                case IdentityResponse.SERVER_BUSY:
                    setNotice("服务器繁忙");
                    break;
                case IdentityResponse.GAME_SERVER_ERROR:
                    setNotice("游戏服务器故障");
                    break;
                case IdentityResponse.ID_CARD_CHECK_PENDING:
                    setNotice("认证中！稍后再试");
                    break;
                case IdentityResponse.ID_CARD_OVER_COUNT:
                    setNotice("认证次数超限");
                    break;
                case IdentityResponse.ID_CARD_CHECK_FAILED:
                case IdentityResponse.ERROR:
                    setNotice("认证失败");
                    break;
                default:
                    setNotice("未知错误 错误码");
                    break;
            }
        }
    }
}