using System;
using System.Collections.Generic;
using UnityEngine;
/*
                   _ooOoo_
                  o8888888o
                  88" . "88
                  (| -_- |)
                  O\  =  /O
               ____/`---'\____
             .'  \\|     |//  `.
            /  \\|||  :  |||//  \
           /  _||||| -:- |||||-  \
           |   | \\\  -  /// |   |
           | \_|  ''\---/''  |   |
           \  .-\__  `-`  ___/-. /
         ___`. .'  /--.--\  `. . __
      ."" '<  `.___\_<|>_/___.'  >'"".
     | | :  `- \`.;`\ _ /`;.`/ - ` : | |
     \  \ `-.   \_ __\ /__ _/   .-` /  /
======`-.____`-.___\_____/___.-`____.-'======
                   `=---='
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            佛祖保佑       永无BUG
*/
namespace SDKFramework
{
    /// <summary>
    /// Author  : mrq
    /// Time    : 2023/12/22
    /// Feature : Frame driver layer, technical support contact Meng Ruiqing
    /// </summary>
    public sealed class ModuleDriver
    {
        public static ModuleDriver Instance { get; private set; }
        public static bool Initialized { get; private set; }
        
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