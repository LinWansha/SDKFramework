using UnityEngine;

namespace SDKFramework
{
    /// <summary>
    /// 作者: mrq
    /// 时间: 2024/01/02
    /// 功能: 
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