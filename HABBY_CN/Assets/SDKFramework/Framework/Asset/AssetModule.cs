using UnityEngine;


namespace SDKFramework.Asset
{
    public partial class AssetModule : BaseModule
    {
#if UNITY_EDITOR
        public const string BUNDLE_LOAD_NAME = "Tools/Build/Bundle Load";
#endif

        public Transform usingObjectRoot;
        public Transform releaseObjectRoot;

        protected internal override void OnModuleUpdate(float deltaTime)
        {
            base.OnModuleUpdate(deltaTime);
            UpdateGameObjectRequests();
        }


    }
}
