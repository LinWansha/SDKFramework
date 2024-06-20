using System.Collections.Generic;
using SDKFramework.Utils;

namespace SDKFramework.Analytics
{
   
    public partial class AnalyticsModule:Singleton<AnalyticsModule>
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