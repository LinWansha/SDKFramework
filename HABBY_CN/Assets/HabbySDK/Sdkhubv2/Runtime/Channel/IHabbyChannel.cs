using System;

namespace Sdkhubv2.Runtime.Platform.Channel
{
    public interface IHabbyChannel
    { 
        int GetChannelId();
        string GetChannelName();
        int GetPayType();
        void Login(string msg, Action<int, string> onResult, Action<int, string> onError);
        void Logout(Action<int, string> onResult, Action<int, string> onError);
        void Pay(string msg, Action<int, string> onResult, Action<int, string> onError);
        void reportGameEvent(string eventName, string msg);
        void initAds(string msg, Action<int, string> onResult, Action<int, string> onError);
        void loadAd(string adId, Action<int, string> onResult, Action<int, string> onError);
        void showAd(string adId, Action<int, string> onResult, Action<int, string> onError);
        void openMarket(string msg);
    }
}