namespace Sdkhubv2.Runtime.Platform
{
    public interface IFuntionHandler
    {
        void onResult(int code,string msg);
        void onError(int code,string msg);
        int getRequestId();
    }
}