using System;

namespace Sdkhubv2.Runtime.Platform.Channel
{
    public class ChannelEditor:IHabbyChannel
    {
        public int GetChannelId()
        {
            throw new NotImplementedException();
        }

        public string GetChannelName()
        {
            throw new NotImplementedException();
        }

        public int GetPayType()
        {
            throw new NotImplementedException();
        }

        public void Login(string msg, Action<int, string> onResult, Action<int, string> onError)
        {
            throw new NotImplementedException();
        }

        public void Logout(Action<int, string> onResult, Action<int, string> onError)
        {
            throw new NotImplementedException();
        }

        public void Pay(string msg, Action<int, string> onResult, Action<int, string> onError)
        {
            throw new NotImplementedException();
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