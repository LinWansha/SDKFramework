using SDKFramework.UI;
using UnityEngine;
using UnityEngine.UI;

[UIView(typeof(SplashAdviceMediator), UIViewID.SplashAdviceUI)]
public class SplashAdviceView : UIView
{
    [SerializeField] internal Text[] texts;
}
