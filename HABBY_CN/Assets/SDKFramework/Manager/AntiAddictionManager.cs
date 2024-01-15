
using Habby.CNUser;
using UnityEngine;

namespace SDKFramework.Manager
{
    public class AntiAddictionManager:MonoSingleton<AntiAddictionManager>
    {
        private void Awake()
        {
#if USE_ANTIADDICTION_TIME
            string timeListenerGoName = "HabbyAntiAddictionTimeListener";
            GameObject timeListenerGo = GameObject.Find(timeListenerGoName);
            if (timeListenerGo == null)
            {
                timeListenerGo = new GameObject(timeListenerGoName);
            }

            HabbyAntiAddictionTimeListener timeListener = timeListenerGo.GetComponent<HabbyAntiAddictionTimeListener>();
            if (timeListener == null)
            {
                timeListener = timeListenerGo.AddComponent<HabbyAntiAddictionTimeListener>();
            }
#endif
#if USE_ANTIADDICTION_PURCHASE
            string purchaseListenerGoName = "HabbyAntiAddictionExpenseListener";
            GameObject purchaseListenerGo = GameObject.Find(purchaseListenerGoName);
            if (purchaseListenerGo == null)
            {
                purchaseListenerGo = new GameObject(purchaseListenerGoName);
            }

            HabbyAntiAddictionExpenseListener purchaseListener =
                purchaseListenerGo.GetComponent<HabbyAntiAddictionExpenseListener>();
            if (purchaseListener == null)
            {
                purchaseListener = purchaseListenerGo.AddComponent<HabbyAntiAddictionExpenseListener>();
            }
#endif
        }
    }
}