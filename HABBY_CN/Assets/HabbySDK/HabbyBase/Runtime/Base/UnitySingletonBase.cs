using System;
using UnityEngine;

namespace Habby.Base
{
    public abstract class UnitySingletonBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region Fields

        /// <summary>
        /// The instance.
        /// </summary>
        private static T instance;

        
        #endregion

        #region Properties
        // 多线程安全机制
        private static readonly object locker = new object();
        
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    lock (locker)
                    {
                        GameObject obj = new GameObject()
                        {
                            name = typeof(T).Name
                        };
                        instance = obj.AddComponent<T>();
                    }
                }
                return instance;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Use this for initialization.
        /// </summary>
        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
                OnAwakeInitialized();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnAwakeInitialized()
        {
        }

        #endregion
    }
}