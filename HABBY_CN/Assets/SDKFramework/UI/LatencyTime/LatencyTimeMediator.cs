using SDKFramework.UI;
using SDKFramework.Utils;
using UnityEngine;

public class LatencyTimeMediator : UIMediator<LatencyTimeView>
{
    private Vector3 _axis = Vector3.forward;

    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
        View.label.text = arg as string;
        Log.Info("LatencyTimeMediator");
        AsyncScheduler.Instance.DelayedInvoke(Close, 2);
    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        View.flower.Rotate(_axis, -0.5f);
    }
}