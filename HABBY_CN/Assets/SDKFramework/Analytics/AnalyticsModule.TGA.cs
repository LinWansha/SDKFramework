using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace SDKFramework.Analytics
{
    public interface TGAImpl
    {
        void UserSet(Dictionary<string, object> properties);
        
        void Track(string eventName);

        void Track(string eventName, Dictionary<string, object> eventParams);

        void SetSuperProperties(Dictionary<string, object> superProperties);
    }
    
    public partial class AnalyticsModule
    {
        enum LoginStep  //TODO：与cp和运营协商 统一放在C#层
        {
            Main=10,
            HabbySDKManager_InitGame=20,
            ReloginOK=25,
            GameManager_StartGame=30,
            GameLoadPanel_Open=40,
            GuideNewPlayer=45,
            MainPanel=50,
        }

        private bool TGAInitialized = false;
        
        private void InitializeTGA()
        {
            if (TGAInitialized)return;
            TGAInitialized = true;
            
            TGA.UserSet(BuildCommonProperties());
            TGA.SetSuperProperties(BuildCommonProperties());
            Dictionary<string, object> BuildCommonProperties()
            {
                return _propertyBuilder
                    .Add("oaid", "unknow")          //国内用户唯一的设备ID
                    .Add("ageLevel", "unknow")      //年龄段
                    .Add("login_type", "unknow")    //weixin/qq/phone/appleid/
                    .Add("tio_id", "unknow")        //热云id
                    .Add("total_iap_cny", 00000)
                    .ToProperty();
            }
        }

        /// <summary>
        /// 首次启动app
        /// </summary>
        public void TGA_first_open()
        {
            if (!TGAInitialized)return;
            TGA.Track("first_open");
        }
        
        /// <summary>
        /// 【SDK登录服】首次登录成功（首次登录服务器分配账号时上报，这个时候还没有通过实名认证）
        /// 1 新设备新账号
        /// 2 老设备新账号
        /// </summary>
        public void TGA_first_login_suc()
        {
            if (!TGAInitialized)return;
            _propertyBuilder.Add("account_type", 1);    //TODO :获取是否为新设备
            TGA.Track("first_login_suc",_propertyBuilder.ToProperty());
        }
        
        /// <summary>
        /// 【服务器】用户首次注册成功，注意国内是实名制成功之后上报
        /// </summary>
        public void TGA_first_active()
        {
            if (!TGAInitialized)return;
            TGA.Track("first_active");
        }
        
        /// <summary>
        /// 用户打开游戏开始加载到登录成功过程中各个步骤成功时上报
        /// </summary>
        public void TGA_log_in(int step,string type,float duration,string login_session_id,bool is_firstopen)
        {
            if (!TGAInitialized)return;
            _propertyBuilder
                .Add("step", step)                          //步骤id【提供配置表】最后一个step建议是进入到游戏内看到主UI的展示
                .Add("type", type)                          //1 必定按顺序触发的步骤  2 可能随机出现的步骤
                .Add("duration", duration)                  //登录过程每个步骤耗时（单位:豪秒）
                .Add("login_session_id", login_session_id)  //每次登录过程记录一个唯一一个id
                .Add("is_firstopen", is_firstopen);         //是否为首次登录
            TGA.Track("log_in",_propertyBuilder.ToProperty());
        }
        
        /// <summary>
        /// 登录过程中发生任何失败时候上报失败原因
        /// </summary>
        public void TGA_log_in_fail(string error_code,int step,string login_session_id)
        {
            if (!TGAInitialized)return;
            _propertyBuilder
                .Add("error_code", error_code)          
                .Add("step", step)                          //步骤id【提供配置表】最后一个step建议是进入到游戏内看到主UI的展示
                .Add("login_session_id", login_session_id); //每次登录过程记录一个唯一一个id
            TGA.Track("log_in_fail",_propertyBuilder.ToProperty());
        }
        private const string EndTimeKey = "last_app_end_time";
        public void TGA_app_start()
        {
            if (!TGAInitialized) return;

            string endTimeStr = PlayerPrefs.GetString(EndTimeKey, null);

            DateTime endTime;
            if (string.IsNullOrEmpty(endTimeStr))
            {
                Log.Warn("No end time recorded.");
                TGA_first_open();
                endTime = DateTime.Now; 
            }
            else if (!DateTime.TryParse(endTimeStr, null, System.Globalization.DateTimeStyles.RoundtripKind, out endTime))
            {
                Log.Error("Failed to parse stored end time.");
                endTime = DateTime.Now; 
            }
    
            DateTime currentTime = DateTime.Now;
            TimeSpan timeDifference = currentTime - endTime;
            float secondsElapsed = (float)timeDifference.TotalSeconds;
            Log.Info($"The time (seconds) elapsed since the last app end{secondsElapsed}");
            
            _propertyBuilder
                .Add("duration", secondsElapsed);
    
            TGA.Track("app_start", _propertyBuilder.ToProperty());
        }
        
        public void TGA_app_end(string type)
        {
            if (!TGAInitialized)return;
            DateTime currentTime = DateTime.Now;
            PlayerPrefs.SetString(EndTimeKey, currentTime.ToString("o")); // 使用 ISO 8601 格式保存，以确保一致性和准确性
            PlayerPrefs.Save();
            _propertyBuilder //
                .Add("duration", Time.realtimeSinceStartup)//游戏时间（本次 app_end与上一次 app_start 的时间差）（单位:秒）
                .Add("type", type)                      //主动退出/切后台/锁屏/杀进程/崩溃
                .Add("last_event_time",DateTime.Now)    //最后一个事件的时间
                .Add("last_event_name","app_end");      //最后一个事件的名称
            TGA.Track("app_end",_propertyBuilder.ToProperty());
        }
        
        public void TGA_iap_show(string product_id,float product_price,float product_price_cny,string pannel_id,string type,int index1,int index2)
        {
            if (!TGAInitialized)return;
            TGA.Track("iap_show",_propertyBuilder.ToProperty());
        }
        
        public void TGA_iap_order()
        {
            if (!TGAInitialized)return;
            TGA.Track("iap_order",_propertyBuilder.ToProperty());
        }
        
        public void TGA_iap_finish_channel()
        {
            if (!TGAInitialized)return;
            TGA.Track("iap_finish_channel",_propertyBuilder.ToProperty());
        }
        
        public void TGA_iap_success()
        {
            if (!TGAInitialized)return;
            TGA.Track("iap_success",_propertyBuilder.ToProperty());
        }
        
        public void TGA_iap_fail()
        {
            if (!TGAInitialized)return;
            TGA.Track("iap_fail",_propertyBuilder.ToProperty());
        }
        
        
        public void TGA_iap_success_server()
        {
            if (!TGAInitialized)return;
            TGA.Track("iap_success_server",_propertyBuilder.ToProperty());
        }
        
        public void TGA_iap_order_server()
        {
            if (!TGAInitialized)return;
            TGA.Track("iap_order_server",_propertyBuilder.ToProperty());
        }
        
        public void TGA_ad_PQ()
        {
            if (!TGAInitialized)return;
            TGA.Track("ad_PQ",_propertyBuilder.ToProperty());
        }
        
        /// <summary>
        /// 国内登录过程关键漏斗，触发时机参考step参数
        /// </summary>
        public void TGA_cn_login()
        {
            if (!TGAInitialized)return;
            _propertyBuilder
                .Add("account_state", "unknown_user")   // unknown_user 未检测到账号 already_hadaccount 检测到账号
                .Add("step", "")                        //TODO: !!!!!!
                .Add("login_type",DateTime.Now)         //step=点击登录方式：appleid/wechat/qq/phone
                .Add("phone_type","app_end")            //phone_quick/phone_normal
                .Add("login_session_id","app_end");     //login_session_id/每次登录过程记录一个唯一一个id
            TGA.Track("cn_login",_propertyBuilder.ToProperty());
        }
    }
}