using SDKFramework.UI;
using UnityEngine;
using UnityEngine.UI;

[UIView(typeof(LatencyTimeMediator), UIViewID.LatencyTimeUI)]
public class LatencyTimeView : UIView
{
    public Text label;
    public Transform flower;
}