using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDKFramework
{
    /// <summary>
    /// 作者: mrq
    /// 时间: 2024/01/02
    /// 功能: 
    /// </summary>
    public sealed class ModuleDriver
    {
        public static ModuleDriver Instance { get; private set; }
        public static bool Initialized { get; private set; }
        //private List<BaseModule> s_modules;
        private Dictionary<Type, BaseModule> m_modules = new Dictionary<Type, BaseModule>();

        public static void Initialize()
        {
            Instance = new ModuleDriver();
        }

        public T GetModule<T>() where T : BaseModule
        {
            if (m_modules.TryGetValue(typeof(T), out BaseModule module))
            {
                return module as T;
            }

            return default(T);
        }

        public void AddModule(BaseModule module)
        {
            Type moduleType = module.GetType();
            if (m_modules.ContainsKey(moduleType))
            {
                Debug.Log($"Module添加失败，重复:{moduleType.Name}");
                return;
            }
            m_modules.Add(moduleType, module);
        }

        public void Update()
        {
            if (!Initialized)
                return;

            if (m_modules == null)
                return;

            if (!Initialized)
                return;

            float deltaTime = UnityEngine.Time.deltaTime;
            foreach (var module in m_modules.Values)
            {
                module.OnModuleUpdate(deltaTime);
            }
        }

        public void LateUpdate()
        {
            if (!Initialized)
                return;

            if (m_modules == null)
                return;

            if (!Initialized)
                return;

            float deltaTime = UnityEngine.Time.deltaTime;
            foreach (var module in m_modules.Values)
            {
                module.OnModuleLateUpdate(deltaTime);
            }
        }

        public void FixedUpdate()
        {
            if (!Initialized)
                return;

            if (m_modules == null)
                return;

            if (!Initialized)
                return;

            float deltaTime = UnityEngine.Time.fixedDeltaTime;
            foreach (var module in m_modules.Values)
            {
                module.OnModuleFixedUpdate(deltaTime);
            }
        }

        public void InitModules()
        {
            if (Initialized)
                return;

            Initialized = true;
            //StartupModules();
            foreach (var module in m_modules.Values)
            {
                module.OnModuleInit();
            }
        }

        public void StartModules()
        {
            if (m_modules == null)
                return;

            if (!Initialized)
                return;

            foreach (var module in m_modules.Values)
            {
                module.OnModuleStart();
            }
        }

        public void Destroy()
        {
            if (!Initialized)
                return;

            if (Instance != this)
                return;

            if (Instance.m_modules == null)
                return;

            foreach (var module in Instance.m_modules.Values)
            {
                module.OnModuleStop();
            }

            //Destroy(Instance.gameObject);
            Instance = null;
            Initialized = false;
        }
    }
}