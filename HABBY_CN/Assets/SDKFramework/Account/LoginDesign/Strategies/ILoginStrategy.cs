using System;
using SDKFramework.Account.Net;

namespace SDKFramework.Account
{
    public interface ILoginStrategy
    {
        void ChannelLogin(Action<LoginResponse> onResponse);
    }

}






