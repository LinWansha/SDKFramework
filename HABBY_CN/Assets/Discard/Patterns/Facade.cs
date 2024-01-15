using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDKFramework
{
    public class Facade : IFacade
    {
        /// <summary>
        /// The Singleton Facade Instance
        /// </summary>
        protected static volatile IFacade m_instance;
        
        /// <summary>
        /// Used for locking the instance calls
        /// </summary>
        protected static readonly object m_staticSyncRoot = new object();
        
        /// <summary>
        /// Facade Singleton Factory method.  This method is thread safe.
        /// </summary>
        public static IFacade Instance
        {
            get
            {
                if (m_instance == null)
                {
                    lock (m_staticSyncRoot)
                    {
                        if (m_instance == null) m_instance = new Facade();
                    }
                }

                return m_instance;
            }
        }
        
        static GameObject m_SdkManager;
        
        static Dictionary<string, object> m_Managers = new Dictionary<string, object>();

        GameObject AppSdkManager {
            get {
                if (m_SdkManager == null) {
                    m_SdkManager = GameObject.Find("HabbyHabbySDK");
                }
                return m_SdkManager;
            }
        }


        public void AddManager(string typeName, object obj) {
            if (!m_Managers.ContainsKey(typeName)) {
                m_Managers.Add(typeName, obj);
            }
        }
        
        
        public T AddManager<T>(string typeName) where T : Component {
            object result = null;
            m_Managers.TryGetValue(typeName, out result);
            if (result != null) {
                return (T)result;
            }
            Component c = AppSdkManager.AddComponent<T>();
            m_Managers.Add(typeName, c);
            return default(T);
        }


        public T GetManager<T>(string typeName) where T : class {
            if (!m_Managers.ContainsKey(typeName)) {
                return default(T);
            }
            object manager = null;
            m_Managers.TryGetValue(typeName, out manager);
            return (T)manager;
        }


        public void RemoveManager(string typeName) {
            if (!m_Managers.ContainsKey(typeName)) {
                return;
            }
            object manager = null;
            m_Managers.TryGetValue(typeName, out manager);
            Type type = manager.GetType();
            if (type.IsSubclassOf(typeof(MonoBehaviour))) {
                GameObject.Destroy((Component)manager);
            }
            m_Managers.Remove(typeName);
        }
    }
}