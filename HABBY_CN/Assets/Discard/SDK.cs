using SDKFramework;
using UnityEngine;

public class SDK : MonoBehaviour
{
    private void Awake()
    {
        SDKFacade.GetInstance().Launch();
    }
}