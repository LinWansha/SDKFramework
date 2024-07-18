namespace Habby.Base
{
    
    public abstract class AbSingletonWithOnCreate<T> where T : class, new()
    {
        #region Fields

        /// <summary>
        /// The instance.
        /// </summary>
        private static T instance;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                    if(instance is IOnCreate)
                    {
                        (instance as IOnCreate).OnCreate();
                    }
                }
                return instance;
            }
        }
        #endregion
    }
}