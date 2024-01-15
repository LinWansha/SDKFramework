using SDKFramework.UI;
using UnityEngine;
using UnityEngine.UI;

[UIView(typeof(EntryMediator), UIViewID.EntryUI)]
public class EntryView :  UIView
{
    public GameObject age8;
    public GameObject age12;
    public GameObject age16;

    public Button btnEnter;
    public Button btnPrivacy;
    public Button btnPersonalInfo;

    public Toggle argeeToggle;
}
