using Habby.CNUser;
using SDKFramework.UI;

public class RealNameMediator : UIMediator<RealNameView>
{
    public const string WrongName = "您输入的姓名有误,请重新输入";
    public const string WrongId = "您输入的身份证号码有误,请重新输入";

    private UserAccount _account;

    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
        view.btConfirm.onClick.AddListener(Confirm);
        view.nameInput.onEndEdit.AddListener(OnEditEnd);
        view.idInput.onEndEdit.AddListener(OnEditEnd);
        AccountManager.OnIdentityFailed += onError;
        AccountManager.OnIdentitySuccess += onSuccess;
        _account=arg as UserAccount;
        if (_account == null)
        {
            Close();
            AccountManager.Instance.CheckUser();
        }
        else
        {
            view.nameInput.text = _account.RealName;
            view.idInput.text = _account.IdCard;
        }
    }
    protected override void OnHide()
    {
        base.OnHide();
        AccountManager.OnIdentityFailed -= onError;
        AccountManager.OnIdentitySuccess -= onSuccess;
    }
    

    private void setNotice(string text)
    {
        view.notice.gameObject.SetActive(true);
        view.notice.text = text;
    }
    
    public void Confirm()
    {
        if (!LocalIdentityUtil.IsChineseName(view.nameInput.text))
        {
            view.notice.text = WrongName;
            view.notice.gameObject.SetActive(true);
            //TrackAdapter.Instance.track_cn_verify_result("SUBMIT","LOCALFAILED",0,"inout name empty or too long");
            return;
        }

        if (string.IsNullOrEmpty(view.idInput.text)||view.idInput.text.Length != 15 && view.idInput.text.Length != 18)
        {
            view.notice.text = WrongId;
            view.notice.gameObject.SetActive(true);
            //TrackAdapter.Instance.track_cn_verify_result("SUBMIT","LOCALFAILED",0,"inout id is too short,length less than 15");
            return;
        }

        if (_account == null)
        {
            AccountManager.Instance.CheckUser();
            //TrackAdapter.Instance.track_cn_verify_result("SUBMIT","NO_DATA",0,"can not find account");
            return;
        }

        _account.RealName = view.nameInput.text;

        string id = view.idInput.text.Trim();
        if (id.Contains("x"))
        {
            id = id.ToUpper();
        }

        _account.IdCard = id;

        //LoginManager.Instance.ValidateIdentity(_account);
        AccountManager.Instance.LocalValidateIdentity(_account);
    }

    private void OnEditEnd(string arg0)
    {
        if (view.notice && view.notice.gameObject.activeSelf)
            view.notice.gameObject.SetActive(false);
    }

   

    private void onSuccess()
    {
        Close();
    }

    private void onError(int code)
    {
         HabbyTextHelper.Instance.ShowTip("实名认证失败!错误代码：" + code);
        // switch (code)
        // {
        //     case IdentityResponse.PARAM_ERROR:
        //         setNotice("输入参数错误");
        //         break;
        //     case IdentityResponse.USER_NOT_FOUND:
        //         setNotice("找不到此用户");
        //         break;
        //     case IdentityResponse.ID_CARD_EXIST:
        //         setNotice(binded);
        //         break;
        //     case IdentityResponse.TOKEN_EXPIRE:
        //         setNotice("登陆已过期,请从新登陆");
        //         break;
        //     case IdentityResponse.SERVER_FATAL_ERROR:
        //         setNotice("GM 服务器故障");
        //         break;
        //     case IdentityResponse.SERVER_BUSY:
        //         setNotice("服务器繁忙");
        //         break;
        //     case IdentityResponse.GAME_SERVER_ERROR:
        //         setNotice("游戏服务器故障");
        //         break;
        //     case IdentityResponse.ID_CARD_CHECK_PENDING:
        //         setNotice("认证中！稍后再试");
        //         break;
        //     case IdentityResponse.ID_CARD_OVER_COUNT:
        //         setNotice("认证次数超限");
        //         break;
        //     case IdentityResponse.ID_CARD_CHECK_FAILED:
        //     case IdentityResponse.ERROR:
        //         setNotice("认证失败");
        //         break;
        //     default:
        //         setNotice("未知错误 错误码");
        //         break;
        // }
        switch (code)
        {
            case 1001:
                setNotice("认证次数超限");
                break;
        }
    }
}