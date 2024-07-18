using SDKFramework.UI;
using UnityEngine.UI;

[UIView(typeof(FreeSmsUseUpMediator), UIViewID.FreeSmsUseUpUI)]
public class FreeSmsUseUpView : UIView
{
    public UIText content;
    
    public Button btnSend;
    
    public Button btnCancel;
}