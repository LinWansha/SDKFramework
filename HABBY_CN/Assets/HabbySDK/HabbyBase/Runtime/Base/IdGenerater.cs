namespace Habby.Base
{
    public static class IdGenerater
    {
        private static long _idStart = 0;
        public static long GenerateInstanceId()
        {
            return _idStart++;
        }
    }
}