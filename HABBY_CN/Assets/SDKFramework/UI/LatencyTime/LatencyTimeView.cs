using SDKFramework.UI;
using UnityEngine;
using UnityEngine.UI;

[UIView(typeof(LatencyTimeMediator), UIViewID.LatencyTimeUI)]
public class LatencyTimeView : UIView
{
    public UIText label;
    public Transform flower;
}