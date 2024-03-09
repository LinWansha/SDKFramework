using SDKFramework.UI;
using UnityEngine.UI;

[UIView(typeof(CrashMediator), UIViewID.CrashUI)]
public class CrashView : UIView
{
    public Text notice;
    public Text detail;
    public Button btnSure;
}