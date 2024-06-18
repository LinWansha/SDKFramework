using System.Collections.Generic;
using SDKFramework.Utils;

namespace SDKFramework.Analytics
{
    public interface TGAImpl
    {
        void UserSet(Dictionary<string, object> properties);
        
        void Report2TGA(string eventName);

        void Report2TGA(string eventName, Dictionary<string, object> eventParams);
    }
    
    public interface MMPImpl
    {
        void UserSet(Dictionary<string, object> properties);
        
        void Report2MMP(string eventName);

        void Report2MMP(string eventName, Dictionary<string, object> eventParams);
    }
    
    public interface GravityImpl
    {
        void UserSet(Dictionary<string, object> properties);
        
        void Report2Gravity(string eventName);

        void Report2Gravity(string eventName, Dictionary<string, object> eventParams);
    }
    
    public class AnalyticsModule:Singleton<AnalyticsModule>
    {
        private TGAImpl TGA;
        private MMPImpl MMP;
        private GravityImpl Gravity;
        
        private static TGPropertyBuilder _propertyBuilder = new TGPropertyBuilder();

        public void Initialization()
        {
            InitializeTGA();
            InitializeMMP();
            InitializeGravity();
        }


        private void InitializeTGA()
        {
            _propertyBuilder
                .Add("oaid", "unknow")          //国内用户唯一的设备ID
                .Add("ageLevel", "unknow")      //年龄段
                .Add("login_type", "unknow")    //weixin/qq/phone/appleid/
                .Add("tio_id", "unknow")        //热云id
                .Add("total_iap_cny", 00000);   //人民币计充值金额
            TGA?.UserSet(_propertyBuilder.ToProperty());
        }
        
        private void InitializeMMP()
        {
        }
        
        private void InitializeGravity()
        {
        }
    }
    
    public class TGPropertyBuilder
    {

        private Dictionary<string, object> _param = new Dictionary<string, object>();
        public int Count => _param.Count;

        public TGPropertyBuilder Add(string key, object value)
        {
            _param[key] = value;
            return this;
        }

        public TGPropertyBuilder AddWithCheck(string key, List<Dictionary<string, object>> dictList)
        {
            if (dictList != null && dictList.Count > 0)
            {
                _param[key] = dictList;
            }
            return this;
        }
        
        public Dictionary<string, object> ToProperty()
        {
            var dict = new Dictionary<string, object>(_param);
            _param.Clear();
            return dict;
        }

        public static void MergeDictionary(Dictionary<string, object> root, Dictionary<string, object> addition)
        {
            foreach (var kv in addition)
                root[kv.Key] = kv.Value;
        }
    }
}