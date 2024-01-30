using Habby.CNUser;
using Newtonsoft.Json;
using SDKFramework.UI;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using SDKFramework.Config;

public class EntryMediator : UIMediator<EntryView>
{
    private AgeTipConfig _configData;

    protected override void OnInit(EntryView view)
    {
        base.OnInit(view);

        view.StartCoroutine(UIConfig.DeserializeByFile($"{HabbyFramework.Asset.SDKConfigPath}App.json", (jsonStr) =>
        {
            _configData = JsonConvert.DeserializeObject<AgeTipConfig>(jsonStr);

            view.ageTip.GetComponent<Image>().sprite =
                HabbyFramework.Asset.LoadAssets<Sprite>("TexTures/" + (int)_configData.applicableRange);

            view.ageTip.onClick.AddListener(() =>
            {
                HabbyFramework.UI.OpenUI(UIViewID.AgeTipUI);
                HabbyFramework.Message.Post(_configData);
            });
        }));

        view.btnEnter.onClick.AddListener(EnterGameOrLogin);
    }

    private void EnterGameOrLogin()
    {
        if (view.argeeToggle.isOn == false)
        {
            HabbyTextHelper.Instance.ShowTip("请勾选用户协议");
        }
        else
        {
            if (AccountManager.Instance.HasAccount)
            {
                Debug.Log("登录");
                AccountManager.Instance.LoginOrIdentify(AccountManager.Instance.CurrentAccount);
            }
            else
            {
                HabbyFramework.UI.OpenUI(UIViewID.LoginUI);
            }
        }
    }

    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
    }
}