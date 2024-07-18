using UnityEngine;

namespace Habby.Base
{
    public class CMonoInstance<T> : MonoBehaviour where T : CMonoInstance<T>
    {
        private static bool _appQuit = false;
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_appQuit)
                {
                    return _instance;
                }
                if (_instance == null)
                {
                    new GameObject(typeof(T).Name).AddComponent<T>();
                }

                return _instance;
            }
        }

        protected virtual void OnInstanceCreate()
        {

        }

        protected virtual void OnInstanceDestroy()
        {

        }

        void Awake()
        {
            _instance = this as T;
            DontDestroyOnLoad(_instance);

            OnInstanceCreate();
        }

        void OnDestroy()
        {
            OnInstanceDestroy();
            _instance = null;
        }

        void OnApplicationQuit()
        {
            //_appQuit = true;
        }
    }
}