using System;
using Habby.Events;

namespace Sdkhubv2.Runtime.Platform.Channel
{
    public class ChannelOfficial:IHabbyChannel
    {
        public int GetChannelId()
        {
            return (int)EHabbyChannel.Official;
        }

        public string GetChannelName()
        {
            return "official";
        }

        public int GetPayType()
        {
            throw new NotImplementedException();
        }

        public void Login(string msg, Action<int, string> onResult, Action<int, string> onError)
        {
            // HabbyEventManagerV1.Instance.Dispatch(GameEventNames.EVENT_LOGOUT);
            HabbyFramework.Account.Login(onResult,onError);
        }

        public void Logout(Action<int, string> onResult, Action<int, string> onError)
        {
            //TODO add logout logic
            HabbyFramework.Account.Logout(1);
            onResult?.Invoke(0, "logout success");
            HabbyEventManagerV1.Instance.Dispatch(GameEventNames.EVENT_LOGOUT);
        }

        public void Pay(string msg, Action<int, string> onResult, Action<int, string> onError)
        {
            //TODO:选择登录方式
            // 目前官网只支持支付宝
            HabbySDKHubManager.Instance.PlatformBridge.Pay(msg, onResult, onError);
        }

        public void reportGameEvent(string eventName, string msg)
        {
            throw new NotImplementedException();
        }

        public void initAds(string msg, Action<int, string> onResult, Action<int, string> onError)
        {
            throw new NotImplementedException();
        }

        public void loadAd(string adId, Action<int, string> onResult, Action<int, string> onError)
        {
            throw new NotImplementedException();
        }

        public void showAd(string adId, Action<int, string> onResult, Action<int, string> onError)
        {
            throw new NotImplementedException();
        }

        public void openMarket(string msg)
        {
            throw new NotImplementedException();
        }
    }
}