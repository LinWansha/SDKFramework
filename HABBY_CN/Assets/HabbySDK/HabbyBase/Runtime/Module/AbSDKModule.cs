using System.Collections.Generic;
using System.Data;
using Habby.Events;

namespace HabbySDK.HabbyBase.Runtime.Module
{
    public abstract class AbSDKModule: ISDKModule
    {

        #region module 
        private static Dictionary<string, ISDKModule> _modules = new Dictionary<string, ISDKModule>();
        #endregion
        
        protected List<string> _dependencyModuleNames;
        protected bool _isReady;
        
        
        

        public AbSDKModule()
        {
            OnCreated();
        }
        
        #region interface

        public virtual List<string> GetDependencyModuleNames()
        {
            return _dependencyModuleNames;
        }

        public virtual void OnCreated()
        {
            _dependencyModuleNames = new List<string>();
            if (!_modules.ContainsKey(GetModuleName()))
            {
                _modules.Add(GetModuleName(),this);
            }
            else
            {
                #if ENABLE_DEBUG
                throw new DuplicateNameException($"module name is duplicate:${GetModuleName()}");
                #endif
                _modules[GetModuleName()] = this;
            }
        }

        public virtual void OnInitialize()
        {
            if (_dependencyModuleNames.Count > 0)
            {
                foreach (var dependencyModuleName in _dependencyModuleNames)
                {
                    ISDKModule module = GetModule(dependencyModuleName);
                    if (module == null || (module != null && !module.IsReady()))
                    {
                        HabbyEventManagerV1.Instance.AddListener<string,bool>(SDKEventNames.GetModuleReadyEvent(dependencyModuleName), OnDependencyModuleReady);
                    }
                }
            }
        }

        public virtual void OnDestroy()
        {
            _isReady = false;
            if (_dependencyModuleNames.Count > 0)
            {
                foreach (var dependencyModuleName in _dependencyModuleNames)
                {
                    ISDKModule module = GetModule(dependencyModuleName);
                    HabbyEventManagerV1.Instance.RemoveListener<string,bool>(SDKEventNames.GetModuleReadyEvent(dependencyModuleName), OnDependencyModuleReady);
                }
            }
        }

        public virtual string GetModuleName()
        {
            return GetType().Name;
        }

        public bool IsReady()
        {
            int readyCount = 0;
            if (GetDependencyModuleNames().Count > 0)
            {
                foreach (string moduleName in GetDependencyModuleNames())
                {
                    if (!_modules.ContainsKey(moduleName))
                    {
                        return false;
                    }
                    if (_modules[moduleName].IsReady())
                    {
                        readyCount++;
                    }
                    else
                    {
                        return false;
                    }
                }

                return readyCount == GetDependencyModuleNames().Count;
            }

            return true;
        }

        #endregion
        
        #region event
        protected void OnDependencyModuleReady(string moduleName,bool isReady)
        {
            
        }
        #endregion

        #region module

        private ISDKModule GetModule(string moduleName)
        {
            if (_modules.ContainsKey(moduleName))
            {
                return _modules[moduleName];
            }

            return null;
        }
        
        private bool HasModule(string moduleName)
        {
            return _modules.ContainsKey(moduleName);
        }

        #endregion
    }
}