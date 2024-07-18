using System;
using System.Collections.Generic;
using Habby.Base;
using UnityEngine;

namespace Habby.Events
{
    public class EventHandlerGroup : IReuse
    {
        #region cache

        private static readonly HabbyLinkListObjectPool<EventHandler> _objectPool = new HabbyLinkListObjectPool<EventHandler>(50,false);
        public static void ClearCache()
        {
            _objectPool.Clear();
        }

        #endregion
       

        /// <summary>
        /// 事件名
        /// </summary>
        public string EventName { get; internal set; }

        /// <summary>
        /// 事件处理器列表
        /// </summary>
        protected List<EventHandler> Handlers { get; private set; }

        /// <summary>
        /// 事件处理器数量
        /// </summary>
        public int Count
        {
            get { return Handlers.Count; }
        }

        /// <summary>
        /// 是否需要排序
        /// </summary>
        public bool NeedSort { get; internal set; }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="type">事件枚举类型</param>
        /// <param name="eventType">事件类型值</param>
        public EventHandlerGroup()
        {
            Handlers = new List<EventHandler>();
            NeedSort = false;
        }

        public void SetEventName(string eventName)
        {
            EventName = eventName;
        }

        /// <summary>
        /// 添加到事件组
        /// </summary>
        /// <param name="eventHandler">事件处理器</param>
        public void Add(EventHandler eventHandler)
        {
            Handlers.Add(eventHandler);
           
            if (eventHandler.Priority != 0 && !NeedSort)
            {
                NeedSort = true;
            }

            if (NeedSort)
            {
                SortEvents();
            }

            EventProcessCallback.OnAdded?.Invoke(eventHandler);
        }

        /// <summary>
        /// 从事件组移除
        /// </summary>
        /// <param name="eventHandler">事件处理器</param>
        public void Remove(EventHandler eventHandler)
        {
            Handlers.Remove(eventHandler);
            _objectPool.Return(eventHandler);
            EventProcessCallback.OnRemoved?.Invoke(eventHandler);
        }

        public bool RemoveByAction(Action action)
        {
            if (Count > 0)
            {
                for (var i = Handlers.Count - 1; i >= 0; i--)
                {
                    var eventHandler = Handlers[i];
                    if (eventHandler.Method.Equals(action.Method) && eventHandler.Target.Equals(action.Target))
                    {
                        Remove(eventHandler);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool RemoveById(long id)
        {
            if (Count > 0)
            {
                for (var i = Handlers.Count - 1; i >= 0; i--)
                {
                    var eventHandler = Handlers[i];
                    if (eventHandler.handlerId == id)
                    {
                        Remove(eventHandler);
                        return true;
                    }
                }
            }
            return false;
        }
        
        public bool RemoveByAction<T>(Action<T> action)
        {
            if (Count > 0)
            {
                for (var i = Handlers.Count - 1; i >= 0; i--)
                {
                    var eventHandler = Handlers[i];
                    if (eventHandler.Method.Equals(action.Method) && eventHandler.Target.Equals(action.Target))
                    {
                        Remove(eventHandler);
                        return true;
                    }
                }
            }
            return false;
        }
        
        public bool RemoveByAction<T1,T2>(Action<T1,T2> action)
        {
            if (Count > 0)
            {
                for (var i = Handlers.Count - 1; i >= 0; i--)
                {
                    var eventHandler = Handlers[i];
                    if (eventHandler.Method.Equals(action.Method) && eventHandler.Target.Equals(action.Target))
                    {
                        Remove(eventHandler);
                        return true;
                    }
                }
            }
            return false;
        }
        
        public bool RemoveByAction<T1,T2,T3>(Action<T1,T2,T3> action)
        {
            if (Count > 0)
            {
                for (var i = Handlers.Count - 1; i >= 0; i--)
                {
                    var eventHandler = Handlers[i];
                    if (eventHandler.Method.Equals(action.Method) && eventHandler.Target.Equals(action.Target))
                    {
                        Remove(eventHandler);
                        return true;
                    }
                }
            }
            return false;
        }
        

        /// <summary>
        /// 事件列表按优先级排序
        /// </summary>
        public void SortEvents()
        {
            Handlers.Sort((e1, e2) => e2.Priority - e1.Priority);
        }
        
        
        public void Dispatch(bool safeDispatch, params object[] args)
        {
            #if ENABLE_DEBUG
            try
            {
                if(args != null && args.Length > 0)
                    Debug.Log($"Dispatch EventName:{EventName} args:{args[0]}");
                else
                    Debug.Log($"Dispatch EventName:{EventName} args:null");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            #endif
            if (null != Handlers)
            {
                List<EventHandler> handlersList = new List<EventHandler>(Handlers);
                
                for (var i = 0; i < handlersList.Count; i++)
                {
                    var eventHandler = handlersList[i];
                    if (safeDispatch)
                    {
                        HabbyEventManagerV1.Instance.ExecuteUpdate(() =>
                        {
                            eventHandler.Invoke(args);
                            CheckHandlerRemainCount(eventHandler);
                        });
                    }
                    else
                    {
                        eventHandler.Invoke(args);
                        CheckHandlerRemainCount(eventHandler);
                    }

                    if (eventHandler.Interrupt) break;
                }
            }
  
        }
        
        //create function with param EventHandler,check the EventHandler's remainCount,if it is 0,remove it
        public void CheckHandlerRemainCount(EventHandler eventHandler)
        {
            if (eventHandler.RemainCount == 0)
                Remove(eventHandler);
        }

        public void Reset()
        {
            if (null != Handlers)
            {
                for (var i = Handlers.Count - 1; i >= 0; i--)
                {
                    Remove(Handlers[i]);
                }
                Handlers.Clear();
            }
        }

        public void Dispose()
        {
            Reset();
        }

        public long AddHandler( Action action ,int remainCount = -1, int priority = 0, bool interrupt = false)
        {
            EventHandler eventHandler = _createEventHandler(action, priority, interrupt, remainCount);
            Add(eventHandler);
            return eventHandler.handlerId;
        }
        
        public long AddHandler<T>( Action<T> action,int remainCount = -1, int priority = 0, bool interrupt = false)
        {
            EventHandler eventHandler = _createEventHandler(action, priority, interrupt, remainCount);
            Add(eventHandler);
            return eventHandler.handlerId;
        }
        
        public long AddHandler<T1,T2>( Action<T1,T2> action ,int remainCount = -1, int priority = 0, bool interrupt = false)
        {
            EventHandler eventHandler = _createEventHandler(action, priority, interrupt, remainCount);
            Add(eventHandler);
            return eventHandler.handlerId;
        }
        
        public long AddHandler<T1,T2,T3>( Action<T1,T2,T3> action ,int remainCount = -1, int priority = 0, bool interrupt = false)
        {
            EventHandler eventHandler = _createEventHandler(action, priority, interrupt, remainCount);
            Add(eventHandler);
            return eventHandler.handlerId;
        }
        
        private EventHandler _createEventHandler<T1,T2,T3>(Action<T1,T2,T3> action , int priority = 0, bool interrupt = false ,int remainCount = -1)
        {
            var eventHandler =  _objectPool.Get();
            eventHandler.EventName = EventName;
            eventHandler.Target = action.Target;
            eventHandler.Priority = priority;
            eventHandler.Interrupt = interrupt;
            eventHandler.Method = action.Method;
            eventHandler.Parameters = action.Method.GetParameters();
            eventHandler.RemainCount = remainCount;
            eventHandler.handlerId = IdGenerater.GenerateInstanceId();
            return eventHandler;
        }
        
        private EventHandler _createEventHandler<T1,T2>(Action<T1,T2> action, int priority = 0, bool interrupt = false,int remainCount = -1)
        {
            var eventHandler =  _objectPool.Get();
            eventHandler.EventName = EventName;
            eventHandler.Target = action.Target;
            eventHandler.Priority = priority;
            eventHandler.Interrupt = interrupt;
            eventHandler.Method = action.Method;
            eventHandler.Parameters = action.Method.GetParameters();
            eventHandler.RemainCount = remainCount;
            eventHandler.handlerId = IdGenerater.GenerateInstanceId();
            return eventHandler;
        }
        
        private EventHandler _createEventHandler(Action action, int priority = 0, bool interrupt = false,int remainCount = -1)
        {
            var eventHandler =  _objectPool.Get();
            eventHandler.EventName = EventName;
            eventHandler.Target = action.Target;
            eventHandler.Priority = priority;
            eventHandler.Interrupt = interrupt;
            eventHandler.Method = action.Method;
            eventHandler.Parameters = action.Method.GetParameters();
            eventHandler.RemainCount = remainCount;
            eventHandler.handlerId = IdGenerater.GenerateInstanceId();
            return eventHandler;
        }
        
        private EventHandler _createEventHandler<T>(Action<T> action, int priority = 0, bool interrupt = false,int remainCount = -1)
        {
            var eventHandler =  _objectPool.Get();
            eventHandler.EventName = EventName;
            eventHandler.Target = action.Target;
            eventHandler.Priority = priority;
            eventHandler.Interrupt = interrupt;
            eventHandler.Method = action.Method;
            eventHandler.Parameters = action.Method.GetParameters();
            eventHandler.RemainCount = remainCount;
            eventHandler.handlerId = IdGenerater.GenerateInstanceId();
            return eventHandler;
        }

 
    }
}