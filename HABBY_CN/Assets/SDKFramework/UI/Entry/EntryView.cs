using SDKFramework.UI;
using UnityEngine;
using UnityEngine.UI;

[UIView(typeof(EntryMediator), UIViewID.EntryUI)]
public class EntryView :  UIView
{
    public Button ageTip;

    public Button btnEnter;
    public Button btnPrivacy;
    public Button btnPersonalInfo;

    public Toggle argeeToggle;
}
