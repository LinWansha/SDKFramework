using SDKFramework.UI;
using UnityEngine;

public class LatencyTimeMediator : UIMediator<LatencyTimeView>
{
    private Vector3 _axis = Vector3.forward;

    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
        view.label.text = arg as string;
        HLogger.Log("LatencyTimeMediator");
    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        view.flower.Rotate(_axis, -1f);
    }
}