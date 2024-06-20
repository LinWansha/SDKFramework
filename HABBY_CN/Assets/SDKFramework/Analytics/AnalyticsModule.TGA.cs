using System.Collections.Generic;

namespace SDKFramework.Analytics
{
    public interface TGAImpl
    {
        void UserSet(Dictionary<string, object> properties);
        
        void Track(string eventName);

        void Track(string eventName, Dictionary<string, object> eventParams);
    }
    
    public partial class AnalyticsModule
    {
        private void InitializeTGA()
        {
            _propertyBuilder
                .Add("oaid", "unknow")          //国内用户唯一的设备ID
                .Add("ageLevel", "unknow")      //年龄段
                .Add("login_type", "unknow")    //weixin/qq/phone/appleid/
                .Add("tio_id", "unknow")        //热云id
                .Add("total_iap_cny", 00000);   //人民币计充值金额
            TGA.UserSet(_propertyBuilder.ToProperty());
        }
        
    }
}