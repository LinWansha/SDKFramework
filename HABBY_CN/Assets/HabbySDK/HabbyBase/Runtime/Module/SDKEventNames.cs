namespace Habby.Events
{
    public class SDKEventNames
    {
        private static readonly string EVENT_SDK_READY = "event.event.sdkReady";

        public static string GetModuleReadyEvent(string moduleName)
        {
            return $"${EVENT_SDK_READY}_{moduleName}";
        }
    }
}