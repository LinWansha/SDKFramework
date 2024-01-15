using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Habby.CNUser
{
    public class HabbyAntiAddictionTimeListener : MonoBehaviour
    {
        private void OnEnable()
        {
            AccountManager.OnNoTimeLeft += onNoTimeLeft;
        }

        private void OnDisable()
        {
            AccountManager.OnNoTimeLeft -= onNoTimeLeft;
        }




        private void onNoTimeLeft(UserAccount account)
        {
            HabbyFramework.UI.OpenUI(UIViewID.NoTimeLeftUI);
        }
    }
}
