using UnityEngine;


namespace SDKFramework.Asset
{
    public partial class AssetModule : BaseModule
    {
        public static readonly string ConfigPath = $"Assets/SDKFramework/Resources/SDKConfig/";
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
