using System.Collections.Generic;

namespace SDKFramework.Analytics
{
    public interface MMPImpl
    {
        void UserSet(Dictionary<string, object> properties);
        
        void Track(string eventName);

        void Track(string eventName, Dictionary<string, object> eventParams);
    }
    
    public partial class AnalyticsModule
    {
        private void InitializeMMP()
        {
        }
        
        /// <summary>
        /// 用户下载完，在联网环境下第一次打开应用（only once）
        /// </summary>
        public void MMP_first_open()
        {
            MMP.Track("first_open");
        }
        
        /// <summary>
        /// 注册+实名认证完成（only once）
        /// </summary>
        public void MMP_login_finish()
        {
            MMP.Track("login_finish");
        }
        
        /// <summary>
        /// 所有付费内购项目
        /// 单位（RMB）
        /// </summary>
        public void MMP_purchase()
        {
            MMP.Track("purchase");
        }
        
        /// <summary>
        /// 用户激活应用后的第二个自然日打开了应用
        /// </summary>
        public void MMP_day1_retention()
        {
            MMP.Track("day1_retention");
        }
        
        /// <summary>
        /// 每个账号首次付费上报一次
        /// </summary>
        public void MMP_first_iap()
        {
            MMP.Track("first_iap");
            
        }
        
        /// <summary>
        /// 每增加付费次数都上报次数
        /// </summary>
        public void MMP_iaptimes_2()
        {
            MMP.Track("iaptimes_2");
        }
    }
}