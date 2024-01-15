using UnityEngine;

namespace SDKFramework
{
    public interface IFacade
    {
        void AddManager(string typeName, object obj);

        T AddManager<T>(string typeName) where T : Component;

        T GetManager<T>(string typeName) where T : class;

        void RemoveManager(string typeName);
    }
}