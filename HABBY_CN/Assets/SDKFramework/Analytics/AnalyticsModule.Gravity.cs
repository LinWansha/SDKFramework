
using System.Collections.Generic;

namespace SDKFramework.Analytics
{
    public interface GravityImpl
    {
        void UserSet(Dictionary<string, object> properties);
        
        void Track(string eventName);

        void Track(string eventName, Dictionary<string, object> eventParams);
    }
    
    public partial class AnalyticsModule
    {
        private void InitializeGravity()
        {
        }
    }
}