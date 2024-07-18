namespace Sdkhubv2.Runtime
{
    public class HabbyGameEvent
    {
        /// <summary>
        ///  QQ登录授权结果: 参数1: code:0 success,参数2: msg:string
        /// </summary>
        public static readonly string EVENT_QQ_LOGIN_RESULT = "event.qq.oauth.result";
        
        public static readonly string EVENT_SDK_SERVER_RESULT = "event.sdk.login.result";
        public static readonly string EVENT_GAME_LOGIN_RESULT = "event.game.login.result";
        public static readonly string EVENT_GAME_PAY_STEP = "event.game.pay.result";
    }
}