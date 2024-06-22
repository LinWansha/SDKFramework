using System.Collections.Generic;
using SDKFramework;
using SDKFramework.Account;
using SDKFramework.Account.DataSrc;
using SDKFramework.UI;
using UnityEngine;
using UnityEngine.UI;
using SDKFramework.Utils;

public class EntryMediator : UIMediator<EntryView>
{
    private AccountModule AccountModule = HabbyFramework.Account;
    private Dictionary<string, LoginChannel> OfficialChannelMap;
    
    protected override void OnInit()
    {
        base.OnInit();
        View.versionName.text = $"版本号：{Application.version}";
        if (Global.App.hasLicense)
        {
            if (Global.Platform==RuntimePlatform.IPhonePlayer)
            {
                View.privacyLine.SetActive(false);
            }
            else
            {
                View.privacyToggle.isOn = false;//todo: 从本地持久化数据拿取
                View.licenseObj.SetActive(true);
                View.btnPrivacy.onClick.AddListener(() => { });//TODO:open webView
                View.btnPersonalInfo.onClick.AddListener(() => { });
            }
        }
        View.privacyToggle.onValueChanged.AddListener((@agree) =>
        {
            HabbyFramework.Account.SetPrivacyStatus(agree);
        });
        OfficialChannelMap = new Dictionary<string, LoginChannel>()
        {
            {UserAccount.ChannelQQ,LoginChannel.QQ},
            {UserAccount.ChannelPhone,LoginChannel.Phone},
            {UserAccount.ChannelWeiXin,LoginChannel.WX},
            {UserAccount.ChannelAppleId,LoginChannel.Apple},
        };
    }

    protected override void OnShow(object arg)
    {
        base.OnShow(arg);

        View.ageTip.GetComponent<Image>().sprite =
            HabbyFramework.Asset.LoadAssets<Sprite>("TexTures/" + (int)Global.App.applicableRange);

        View.btnEnter.onClick.AddListener(EnterGameOrLogin);
        View.ageTip.onClick.AddListener(ShowAgeTip);
    }

    private void ShowAgeTip()
    {
        HabbyFramework.UI.OpenUI(UIViewID.AgeTipUI);
    }
    private void EnterGameOrLogin()
    {
        if (View.privacyToggle.isOn == false)
        {
            HabbyTextHelper.Instance.ShowTip("请勾选用户协议");
            return;
        }

        if (!AccountModule.HasAccount)
            HabbyFramework.UI.OpenUI(UIViewID.LoginUI);
        else
            AccountModule.loginRunner.Execute(OfficialChannelMap[Global.Channel]);
    }

    protected override void OnHide()
    {
        View.btnEnter.onClick.RemoveListener(EnterGameOrLogin);
        View.ageTip.onClick.RemoveListener(ShowAgeTip);
        base.OnHide();
    }

}