using System;
using System.Collections.Generic;
using System.Reflection;
using SDKFramework;
using SDKFramework.Account;
using SDKFramework.Asset;
using SDKFramework.Message;
using SDKFramework.Network;
using SDKFramework.UI;
using UnityEngine;
using UnityEngine.Profiling;

/// <summary>
/// Author  : Meng Ruiqing
/// Time    : 2023/12/23
/// Feature : Framework facade , technical support contact Meng Ruiqing
/// </summary>
public class HabbyFramework : MonoBehaviour
{
    
    [Module(1)] 
    public static AssetModule Asset => ModuleDriver.Instance.GetModule<AssetModule>();

    [Module(2)] 
    public static UIModule UI => ModuleDriver.Instance.GetModule<UIModule>();

    [Module(3)] 
    public static NetworkModule Network => ModuleDriver.Instance.GetModule<NetworkModule>();

    [Module(4)] 
    public static MessageModule Message => ModuleDriver.Instance.GetModule<MessageModule>();
    
    [Module(5)] 
    public static AccountModule Account => ModuleDriver.Instance.GetModule<AccountModule>();
    
    // [Module(6)] 
    // public static AnalyticsModule Analytics => ModuleDriver.Instance.GetModule<AnalyticsModule>();
    

    private bool activing;
    

    private void Awake()
    {
        if (ModuleDriver.Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        activing = true;
        DontDestroyOnLoad(gameObject);


        Application.logMessageReceived += OnReceiveLog;
        ModuleDriver.Initialize();
        StartupModules();
        ModuleDriver.Instance.InitModules();
    }

    private void Start()
    {
        ModuleDriver.Instance.StartModules();
    }

    private void Update()
    {
        ModuleDriver.Instance.Update();
    }

    private void LateUpdate()
    {
        ModuleDriver.Instance.LateUpdate();
    }

    private void FixedUpdate()
    {
        ModuleDriver.Instance.FixedUpdate();
    }

    private void OnDestroy()
    {
        if (activing)
        {
            Application.logMessageReceived -= OnReceiveLog;
            ModuleDriver.Instance.Destroy();
        }
    }


    /// <summary>
    /// 初始化模块
    /// </summary>
    public void StartupModules()
    {
        List<ModuleAttribute> moduleAttrs = new List<ModuleAttribute>();
        PropertyInfo[] propertyInfos =
            GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        Type baseCompType = typeof(BaseModule);
        for (int i = 0; i < propertyInfos.Length; i++)
        {
            PropertyInfo property = propertyInfos[i];
            if (!baseCompType.IsAssignableFrom(property.PropertyType))
                continue;

            object[] attrs = property.GetCustomAttributes(typeof(ModuleAttribute), false);
            if (attrs.Length == 0)
                continue;

            Component comp = GetComponentInChildren(property.PropertyType);
            if (comp == null)
            {
                Debug.LogError($"Can't Find GameModule:{property.PropertyType}");
                continue;
            }

            ModuleAttribute moduleAttr = attrs[0] as ModuleAttribute;
            moduleAttr.Module = comp as BaseModule;
            moduleAttrs.Add(moduleAttr);
        }

        moduleAttrs.Sort();
        for (int i = 0; i < moduleAttrs.Count; i++)
        {
            ModuleDriver.Instance.AddModule(moduleAttrs[i].Module);
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ModuleAttribute : Attribute, IComparable<ModuleAttribute>
    {
        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority { get; private set; }

        /// <summary>
        /// 模块
        /// </summary>
        public BaseModule Module { get; set; }

        /// <summary>
        /// 添加该特性才会被当作模块
        /// </summary>
        /// <param name="priority">控制器优先级,数值越小越先执行</param>
        public ModuleAttribute(int priority)
        {
            Priority = priority;
        }

        int IComparable<ModuleAttribute>.CompareTo(ModuleAttribute other)
        {
            return Priority.CompareTo(other.Priority);
        }
    }

    private void OnReceiveLog(string condition, string stackTrace, LogType type)
    {
#if!UNITY_EDITOR
        if (type == LogType.Exception)
        {
            Log.Error($"{condition}\n{stackTrace}");
        }
#endif
    }
}