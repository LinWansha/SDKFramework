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
                HabbyFramework.UI.OpenUI(UIViewID.SplashAdviceUI);
            },
            Login = (token) =>
            {
                //use this token to login game server
                Debug.Log($"版署服务器登录成功,Persistent token: {token}");
            },
            EnterGame = () =>
            {
                //Write your logic for entering the game
                Debug.Log("宿主程序进入成功!!!");
            },
        });
    }
}