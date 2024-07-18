using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Habby.Events
{
    public class GameEventNames
    {
        #region game event
        /// <summary>
        /// idfa事件，bool result 是否授权（Android 也要有）
        /// </summary>
        public static readonly string EVENT_IDFA_RESULT = "event.event.idfaResult";

        // /// <summary>
        // ///  程序启动
        // /// </summary>
        // public static readonly string EVENT_APP_START = "event.event.appStart";


        /// <summary>
        /// ApplicationFocus: bool resume_from_background
        /// </summary>
        public static readonly string EVENT_APP_FOCUS_CHANGE = "event.event.appFocusChange";

        /// <summary>
        /// 登录游戏服务器:string userId,bool isNewUser
        /// </summary>
        public static readonly string EVENT_LOGIN_GAME_SERVER = "event.event.loginGameServer";
                
        /// <summary>
        /// 登出游戏服务器
        /// </summary>
        public static readonly string EVENT_LOGOUT = "event.game.logout";
        
        /// <summary>
        /// 服务器认证后的支付:string iapId,float localPrice,bool isSucceed
        /// </summary>
        public static readonly string EVENT_SERVER_CHECKED_PAY = "event.event.serverCheckedPay";


        /// <summary>
        /// 广告奖励 string adTypeName
        /// </summary>
        public static readonly string EVENT_AD_REWARD = "event.event.adReward";


        /// <summary>
        /// 发现本地用户数据[登录loginServer后触发]
        /// </summary>
        public static readonly string EVENT_FOUND_LOCAL_USER_DATA = "event.event.foundLocalUser";

        /// <summary>
        ///  进入游戏Loading进度 int step
        /// </summary>
        public static readonly string EVENT_LOADING_PROCESS = "event.event.loginProcess";


        /// <summary>
        ///  开始加载 
        /// </summary>
        public static readonly string EVENT_LOADING_START = "event.event.loadingStart";


         /// <summary>
        ///  app 关闭 flot duration
        /// </summary>
        public static readonly string EVENT_APP_END = "event.event.appEnd";
        
         
         /// <summary>
         /// 广告加载失败
         /// </summary>
         public static readonly string EVENT_AD_LOAD_FAILED = "event.game.ad.loadFailed";
        #endregion
    }
}