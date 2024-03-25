
using SDKFramework.UI;
using UnityEngine.UI;

[UIView(typeof(RealNameMediator), UIViewID.RealNameUI)]
public class RealNameView : UIView
{
    public InputField nameInput;
    public InputField idInput;

    public Text notice;
    public Button btConfirm;
        
    
}