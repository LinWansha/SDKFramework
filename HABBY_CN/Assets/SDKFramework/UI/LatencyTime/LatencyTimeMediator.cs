using SDKFramework.UI;
using SDKFramework.Utils;
using UnityEngine;

public class LatencyTimeMediator : UIMediator<LatencyTimeView>
{
    private Vector3 _axis = Vector3.forward;

    protected override void OnShow(object arg)
    {
        base.OnShow(arg);

        Log.Info("LatencyTimeMediator");

        if (arg==null)
            View.label.text = "加载中...";
        else
            View.label.text = arg as string;
            
        AsyncScheduler.Instance.DelayedInvoke(Close, 2);
    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        View.flower.Rotate(_axis, -0.5f);
    }
}