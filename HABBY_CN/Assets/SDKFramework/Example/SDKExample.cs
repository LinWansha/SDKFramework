using ChainofResponsibility;
using Habby.CNUser;
using SDKFramework.Account;
using SDKFramework.Config;
using SDKFramework.Message;
using UnityEngine;

public class SDKExample : MonoBehaviour
{
    private static AppConfig AppData;

    void Awake()
    {
        string jsonStr = Resources.Load<TextAsset>("SDKConfig/App").text;
        AppData = JsonUtility.FromJson<AppConfig>(jsonStr);
        HabbyFramework.Message.Post(AppData);
        HabbyFramework.UI.OpenUI(UIViewID.EntryUI);
        
        PurchaseRequest requestTelphone = new PurchaseRequest(4000.0, "Telphone");
        PurchaseRequest requestSoftware = new PurchaseRequest(10000.0, "Visual Studio");
        PurchaseRequest requestComputers = new PurchaseRequest(40000.0, "Computers");

        ChainofResponsibility.Approver manager = new Manager("LearningHard");
        ChainofResponsibility.Approver Vp = new VicePresident("Tony");
        ChainofResponsibility.Approver Pre = new President("BossTom");

        // 设置责任链
        manager.NextApprover = Vp;
        Vp.NextApprover = Pre;

        // 处理请求
        manager.ProcessRequest(requestTelphone);
        manager.ProcessRequest(requestSoftware);
        manager.ProcessRequest(requestComputers);

// SDKFramework.Account.Handler<RegisterResponse> register = new RegisterHandler(response);
        // SDKFramework.Account.Handler<RegisterResponse> register = new RegisterHandler("NAME");
        // SDKFramework.Account.Handler<LoginResponse> login = new LoginHandler("NAME");
        // SDKFramework.Account.Handler<IdentityResponse> identity = new IdentityHandler("NAME");
        //
        // register.NextHandler = login;
        // login.NextHandler = identity;
    }
}

public class AppSource : MessageHandler<AppConfig>
{
    public static AppConfig Data;

    public override void HandleMessage(AppConfig arg)
    {
        Data = arg;
    }
}