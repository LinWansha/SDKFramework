using System;
using System.Collections.Generic;
using System.Reflection;


namespace SDKFramework.Message
{
    /// <summary>
    /// 消息中心，后面如果用的多，可以考虑异步，并引入对象池
    /// </summary>
    public class MessageModule : BaseModule
    {
        public delegate void MessageHandlerEventArgs<T>(T arg);

        private Dictionary<Type, List<object>> globalMessageHandlers;
        private Dictionary<Type, List<object>> localMessageHandlers;

        public Monitor Monitor { get; private set; }

        protected internal override void OnModuleInit()
        {
            base.OnModuleInit();
            localMessageHandlers = new Dictionary<Type, List<object>>();
            Monitor = new Monitor();
            LoadAllMessageHandlers();
        }

        protected internal override void OnModuleStop()
        {
            base.OnModuleStop();
            globalMessageHandlers = null;
            localMessageHandlers = null;
        }

        private void LoadAllMessageHandlers()
        {
            globalMessageHandlers = new Dictionary<Type, List<object>>();
            //Assembly assembly= Assembly.GetExecutingAssembly(); 
            //Type[] arrtypes= Assembly.GetCallingAssembly().GetTypes();
            foreach (var type in Assembly.GetCallingAssembly().GetTypes())
            {
                if (type.IsAbstract)
                    continue;

                MessageHandlerAttribute messageHandlerAttribute =
                    type.GetCustomAttribute<MessageHandlerAttribute>(true);
                if (messageHandlerAttribute != null)
                {
                    IMessageHandler messageHandler = Activator.CreateInstance(type) as IMessageHandler;
                    if (!globalMessageHandlers.ContainsKey(messageHandler.GetHandlerType()))
                    {
                        globalMessageHandlers.Add(messageHandler.GetHandlerType(), new List<object>());
                    }

                    globalMessageHandlers[messageHandler.GetHandlerType()].Add(messageHandler);
                }
            }
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        public void Subscribe<T>(MessageHandlerEventArgs<T> handler)
        {
            Type argType = typeof(T);
            if (!localMessageHandlers.TryGetValue(argType, out var handlerList))
            {
                handlerList = new List<object>();
                localMessageHandlers.Add(argType, handlerList);
            }

            handlerList.Add(handler);
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        public void Unsubscribe<T>(MessageHandlerEventArgs<T> handler)
        {
            if (!localMessageHandlers.TryGetValue(typeof(T), out var handlerList))
                return;

            handlerList.Remove(handler);
        }

        public void Post<T>(T arg) where T : struct
        {
            if (globalMessageHandlers.TryGetValue(typeof(T), out List<object> globalHandlerList))
            {
                foreach (var handler in globalHandlerList)
                {
                    if (!(handler is MessageHandler<T> messageHandler))
                        continue;

                    { messageHandler.HandleMessage(arg);}
                }
            }

            if (localMessageHandlers.TryGetValue(typeof(T), out List<object> localHandlerList))
            {
                List<object> list = new List<object>();
                list.AddRangeNonAlloc(localHandlerList);
                foreach (var handler in list)
                {
                    if (!(handler is MessageHandlerEventArgs<T> messageHandler))
                        continue;

                    { messageHandler(arg);}
                }

                list = null; //TODO：这个地方发消息频繁的话就放到池子里
            }
        }
    }
}