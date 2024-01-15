using SDKFramework.UI;
using UnityEngine;
using UnityEngine.UI;

[UIView(typeof(SplashAdviceMediator), UIViewID.SplashAdviceUI)]
public class SplashAdviceView : UIView
{
    [SerializeField] internal GameObject panel;

    [SerializeField] internal Text[] texts;
    
}
//为了尽可能更早的展示健康游戏忠告，不再从UI管理中打开此面板，目前已经从UI管理中弃用