using SDKFramework;
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
                HabbyFramework.UI.OpenUI(UIViewID.EntryUI);
            },
            Login = () =>
            {
                HabbyFramework.UI.OpenUI(UIViewID.EntryUI);
            },
            EnterGame = () =>
            {
                //Write your logic for entering the game
                HLogger.Log("宿主程序进入成功!!!");
            },
        });
    }
}