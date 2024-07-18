namespace Sdkhubv2.Runtime.Platform
{
    public class HabbyRequestIdUtil
    {
        private static uint  mRequestId = 1000;
        public static uint GetRequestId()
        {
            return mRequestId++;
        }
    }
}