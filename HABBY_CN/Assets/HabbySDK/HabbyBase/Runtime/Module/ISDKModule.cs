using System.Collections.Generic;

namespace HabbySDK.HabbyBase.Runtime.Module
{
    public interface ISDKModule
    {
        List<string> GetDependencyModuleNames();
        void OnCreated();
        void OnInitialize();
        void OnDestroy();
        string GetModuleName();
        
        bool IsReady();
    }
}