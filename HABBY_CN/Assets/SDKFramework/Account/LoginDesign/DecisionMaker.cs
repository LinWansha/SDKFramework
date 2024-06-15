using System.Collections.Generic;
using System.Threading.Tasks;
using SDKFramework.Message;
using SDKFramework.Utils;
using UnityEngine;

namespace SDKFramework.Account
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

        public async void Execute(LoginChannel channel)
        {
            loginStrategy = Strategy_MAP[channel];
            if (!loginStrategy.CheckPrivacyStatus())
            {
                AccountLog.Info($"User has not agreed to privacy policy, cannot login with {channel}");
                HabbyFramework.Message.Post(new MsgType.ShowNoAgreePrivacyNotice());
                return;
            }

            var loginTCS = AsyncScheduler.CreateTCS<bool>();

            loginStrategy.Login(new RespHandler
            {
                success = () =>
                {
                    AccountLog.Info("channel login successful !!!");
                    loginTCS.SetResult(true);
                },
                failed = () =>
                {
                    AccountLog.Warn("channel login failed.");
                    loginTCS.SetResult(false);
                },
            });

            bool loginSuccess = await loginTCS.Task;

            if (!loginSuccess)
            {
                return;
            }

            var validateTCS = AsyncScheduler.CreateTCS<bool>();

            loginStrategy.ValidateIdentity(new RespHandler
            {
                success = () =>
                {
                    AccountLog.Info("identity validation successful !!!");
                    validateTCS.SetResult(true);
                },
                failed = () =>
                {
                    AccountLog.Warn("identity validation failed.");
                    validateTCS.SetResult(false);
                    // Handle identity validation failure if necessary
                },
            });

            bool validateSuccess = await validateTCS.Task;

            if (!validateSuccess)
            {
                // Handle identity validation failure if necessary
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