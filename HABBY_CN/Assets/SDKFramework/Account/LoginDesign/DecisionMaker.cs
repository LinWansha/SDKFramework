using System;
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
                HabbyFramework.Message.Post(new SDKEvent.ShowNoAgreePrivacyNotice());
                return;
            }

            bool loginSuccess = await ExecuteAsync(loginStrategy.Login);
            if (!loginSuccess) return;

            bool validateSuccess = await ExecuteAsync(loginStrategy.ValidateIdentity);
            if (!validateSuccess) return;

            await ExecuteAsync(loginStrategy.RealNameLogin);
        }

        private Task<bool> ExecuteAsync(Action<RespHandler> action)
        {
            var tcs = AsyncScheduler.CreateTCS<bool>();

            action(new RespHandler
            {
                success = () => tcs.SetResult(true),
                failed = () => tcs.SetResult(false),
            });

            return tcs.Task;
        }
        
        // public async void Execute(LoginChannel channel)
        // {
        //     loginStrategy = Strategy_MAP[channel];
        //     if (!loginStrategy.CheckPrivacyStatus())
        //     {
        //         AccountLog.Info($"User has not agreed to privacy policy, cannot login with {channel}");
        //         HabbyFramework.Message.Post(new MsgType.ShowNoAgreePrivacyNotice());
        //         return;
        //     }
        //
        //     var loginTCS = AsyncScheduler.CreateTCS<bool>();
        //
        //     loginStrategy.Login(new RespHandler
        //     {
        //         success = () =>
        //         {
        //             AccountLog.Info("[channel login] successful !!!");
        //             loginTCS.SetResult(true);
        //         },
        //         failed = () =>
        //         {
        //             AccountLog.Warn("[channel login]  failed.");
        //             loginTCS.SetResult(false);
        //         },
        //     });
        //
        //     bool loginSuccess = await loginTCS.Task;
        //
        //     if (!loginSuccess)
        //         return;
        //
        //
        //     var validateTCS = AsyncScheduler.CreateTCS<bool>();
        //
        //     loginStrategy.ValidateIdentity(new RespHandler
        //     {
        //         success = () =>
        //         {
        //             AccountLog.Info("[validate identity] successful !!!");
        //             validateTCS.SetResult(true);
        //         },
        //         failed = () =>
        //         {
        //             AccountLog.Warn("[validate identity] failed.");
        //             validateTCS.SetResult(false);
        //         },
        //     });
        //
        //     bool validateSuccess = await validateTCS.Task;
        //
        //     if (!validateSuccess)
        //         return;
        //
        //     loginStrategy.RealNameLogin(new RespHandler
        //     {
        //         success = () => { AccountLog.Info("[real name login] successful !!!"); },
        //         failed = () => { AccountLog.Warn("[real name login] failed."); },
        //     });
        // }
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