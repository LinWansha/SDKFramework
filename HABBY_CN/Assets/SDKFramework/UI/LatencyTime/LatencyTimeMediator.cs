using SDKFramework.UI;
using UnityEngine;

public class LatencyTimeMediator : UIMediator<LatencyTimeView>
{
    private Vector3 axis = Vector3.forward;

    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
        view.label.text = arg as string;
        HLogger.Log("LatencyTimeMediator");
    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        view.flower.Rotate(axis, -1f);
    }
}