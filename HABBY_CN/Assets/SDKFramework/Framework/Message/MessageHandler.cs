using System;
using System.Threading.Tasks;

namespace SDKFramework.Message
{
    public interface IMessageHander
    {
        Type GetHandlerType();

    }

    [MessageHandler]
    public abstract class MessageHandler<T> : IMessageHander where T : struct
    {
        public Type GetHandlerType()
        {
            return typeof(T);
        }

        public abstract void HandleMessage(T arg);
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    sealed class MessageHandlerAttribute : Attribute { }
}
