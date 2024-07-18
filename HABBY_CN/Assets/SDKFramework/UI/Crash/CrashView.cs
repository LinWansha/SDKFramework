using SDKFramework.UI;
using UnityEngine.UI;

[UIView(typeof(CrashMediator), UIViewID.CrashUI)]
public class CrashView : UIView
{
    public UIText notice;
    public UIText detail;
    public Button btnSure;
}