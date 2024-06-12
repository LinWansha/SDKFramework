using UnityEngine;

namespace SDKFramework
{
    /// <summary>
    /// Author  : Meng Ruiqing
    /// Time    : 2023/12/22
    /// Feature : Modules lifecycle, technical support contact Meng Ruiqing
    /// </summary>
	public abstract class BaseModule : MonoBehaviour
    {
        private void Awake() { }
        private void Start() { }
        private void Update() { }
        private void OnDestroy() { }

        protected internal virtual void OnModuleInit() { }
        protected internal virtual void OnModuleStart() { }
        protected internal virtual void OnModuleStop() { }
        protected internal virtual void OnModuleUpdate(float deltaTime) { }
        protected internal virtual void OnModuleLateUpdate(float deltaTime) { }
        protected internal virtual void OnModuleFixedUpdate(float deltaTime) { }
    }
}