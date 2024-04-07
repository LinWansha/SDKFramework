using SDKFramework;
using SDKFramework.Account.DataSrc;
using UnityEngine;

public class SDKExample : MonoBehaviour
{
    void Start()
    {
        SDK MRQ = SDK.New();
        MRQ.Run(new SDK.ProcedureOption()
        {
            Splash = () =>
            {
                //HabbyFramework.UI.OpenUI(UIViewID.SplashAdviceUI);
            },
            Login = () =>
            {
                HabbyFramework.UI.OpenUI(UIViewID.EntryUI);
                // HabbyFramework.Account.LoginOrIdentify(new UserAccount()
                // {
                //     LoginChannel = UserAccount.ChannelQQ,
                //     AgeRange = UserAccount.AgeLevel.Adult,
                //     UID = "林万厦",
                //     Age = 21
                // });
            },
            EnterGame = () =>
            {
                //Write your logic for entering the game
                HLogger.Log("宿主程序进入成功!!!");
            },
        });

        SDK.Procedure.Login();
        
        
    }
}