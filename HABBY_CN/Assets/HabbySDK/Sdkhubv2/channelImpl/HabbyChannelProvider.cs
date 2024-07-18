using Habby;
using Habby.SDK.Tool;
using Sdkhubv2.Runtime.tools;
using UnityEngine;

namespace Sdkhubv2.Runtime.Platform.Channel
{
    public class HabbyChannelProvider:IChannelProvider
    {
        protected static bool mIsInit = false;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Start()
        {
            if (!mIsInit)
            {
                mIsInit = true;
                HabbySDKHubManager.Instance.ChannelProvider = new HabbyChannelProvider();
            }
        }

        public IHabbyChannel GetChannel()
        {
            Debug.Log($"#HabbyChannelProvider GetChannel:{HabbySDKHubManager.Instance.PlatformBridge.GetChannelId()}");
            #if UNITY_EDITOR
            return new ChannelOfficial();
            #endif
            switch ((EHabbyChannel)HabbySDKHubManager.Instance.PlatformBridge.GetChannelId())
            {
                case EHabbyChannel.Official:
                    return new ChannelOfficial();
                default:
                    return new ChannelOfficial();
            }
        }
    }
}