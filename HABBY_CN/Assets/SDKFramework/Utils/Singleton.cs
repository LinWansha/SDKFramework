namespace SDKFramework.Utils
{
    public class Singleton<T> where T : new()
    {
        private static T _instance;
        private static readonly object objlock = new object();

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (objlock)
                {
                    _instance ??= new T();
                }

                return _instance;
            }
        }
    }
}