using SDKFramework.UI;
using UnityEngine.UI;

[UIView(typeof(NotGameTimeMediator), UIViewID.NotGameTimeUI)]
public class NotGameTimeView : UIView
{
    public Text contentText;
    public Button btnSure;
}