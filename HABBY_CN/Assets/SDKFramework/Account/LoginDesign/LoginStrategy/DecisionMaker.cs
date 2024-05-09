using System.Collections.Generic;
using SDKFramework.Message;
using UnityEngine;

namespace SDKFramework.LoginDesign
{
    /// <summary>
    /// Decision maker, choose specific login strategies
    /// </summary>
    public class DecisionMaker
    {
        private LoginTemplate loginStrategy;

        private readonly Dictionary<LoginChannel, LoginTemplate> Strategy_MAP;

        public DecisionMaker()
        {
            Strategy_MAP = new Dictionary<LoginChannel, LoginTemplate>(4)
            {
                { LoginChannel.QQ, new QQLoginStrategy() },
                { LoginChannel.WX, new WxLoginStrategy() },
                { LoginChannel.Phone, new PhoneLoginStrategy() },
                { LoginChannel.Apple, new AppleLoginStrategy() },
            };
        }

        public void Execute(LoginChannel channel)
        {
            loginStrategy = Strategy_MAP[channel];
            if (loginStrategy.CheckPrivacyStatus())
            {
                loginStrategy.Login();
            }
            else
            {
                HLogger.Log($"User no agree privacy,can not to login with {channel}",Color.yellow);
                HabbyFramework.Message.Post(new MsgType.ShowNoAgreePrivacyNotice());
            }
        }

    }

    /// <summary>
    /// Official login channel
    /// </summary>
    public enum LoginChannel : byte
    {
        QQ,
        WX,
        Phone,
        Apple
    }
}