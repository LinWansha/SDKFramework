using Habby.CNUser;
using SDKFramework.Manager;
using SDKFramework.Net;
using UnityEngine;

namespace SDKFramework
{
    public class SDKFacade : Facade
    {
        static SDKFacade () {
            m_instance = new SDKFacade ();
        }
        public static SDKFacade GetInstance()
        {
            return m_instance as SDKFacade;
        }

        public void Launch()
        {
            Debug.Log("Launch");
            
            AddManager<AccountManager>("Account");
            AddManager<AntiAddictionManager>("AntiAddiction");
      //      AddManager<LoginManager>("Login");
            AddManager<NativeInteract>("Native");
            //AddManager<NetManager>("Net");
        }
    }
}
