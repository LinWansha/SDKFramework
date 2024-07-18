namespace Sdkhubv2.Runtime
{
    public class HabbyApiConst
    {
        #region qq
        public static readonly string API_GET_QQ_LOGIN_TOKEN         = "name.funtion.qq.oauth";
        public static readonly string API_GET_QQ_INSTALLED           = "function.qq.install";
        public static readonly string API_GET_QQ_SUPPORT_SHARE_ZONE  = "function.qq.support.zone";
        public static readonly string API_GET_QQ_SUPPORT_SHARE_QQ    = "function.qq.support.qq";
        public static readonly string API_QQ_LOGOUT                  = "funtion.qq.logout";
        public static readonly string API_ADD_QQ_GROUP               = "funtion.qq.addgroup";

        #endregion

        #region weichat
        public static readonly string API_WEICHAT_OAUTH_GET_TOKEN            = "function.wechat.oauth";
        public static readonly string API_WEICHAT_SHARE                      = "function.wechat.share";
        public static readonly string API_WEICHAT_PAY                        = "function.wechat.pay";
        #endregion

        #region oaid

        public static readonly string API_GET_IS_OAID_SUPPORT      = "funtion.plugin.oaid.issupport";
        public static readonly string API_GET_IS_OAID_READY        = "funtion.plugin.oaid.isready";
        public static readonly string API_GET_OAID                 = "funtion.plugin.oaid.get";
        public static readonly string NOTIFY_OAID_RESULT           = "notify.oaid.result";

        #endregion

        #region shanyan
        public static readonly string API_GET_SHANYAN_TOKEN = "funtion.shanyan.oauth";
        public static readonly string API_HAS_SIMCARD       = "funtion.shanyan.hassim";
        public static readonly string API_IS_READY          = "funtion.shanyan.isready";
        public static readonly string API_GET_STATE         = "funtion.shanyan.state";

        #endregion

        #region pay
        public static readonly string API_CHOOSE_PAYMENT_TYPE = "funtion.paymentchoose";
        #endregion
        
        #region sms
        public static readonly string API_SEND_SMS = "funtion.sms.send";
        #endregion
      
    }
}