using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Habby.Base;
using UnityEngine;

namespace Habby.Events
{
  
    public class EventHandler:IReuse
    {
        #region Field

        /// <summary>
        /// 事件类型
        /// </summary>
        public string EventName;

        /// <summary>
        /// 事件目标对象
        /// </summary>
        public object Target;

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority;

        /// <summary>
        /// 是否中断事件队列
        /// </summary>
        public bool Interrupt;

        /// <summary>
        /// 监听方法
        /// </summary>
        public MethodInfo Method;

        /// <summary>
        /// 监听方法的参数
        /// </summary>
        public ParameterInfo[] Parameters;
        
        /// <summary>
        /// 剩余触发次数
        /// -1 无限次
        /// </summary>
        public int RemainCount = -1;

        public long handlerId = -1;
        #endregion

        #region Invoke

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType">事件类型值</param>
        /// <param name="args">参数</param>
        /// <returns>执行结果</returns>
        public virtual bool Invoke( params object[] args)
        {
            var success = InvokeMethod(this, args);
            EventProcessCallback.OnDispatched?.Invoke(this, args);
            return success;
        }

        /// <summary>
        /// 执行监听方法
        /// </summary>
        /// <param name="eventHandler">监听事件数据</param>
        /// <param name="eventType">事件类型值</param>
        /// <param name="args">事件参数</param>
        /// <returns>执行结果</returns>
        internal static bool InvokeMethod(EventHandler eventHandler,  params object[] args)
        {
            var method = eventHandler.Method;
            var parameters = eventHandler.Parameters;
            var target = eventHandler.Target;
            var fixArgs = new object[eventHandler.Parameters.Length];
            try
            {
                if (eventHandler.Method == null)
                {
                    EventProcessCallback.OnError?.Invoke(eventHandler, args, new Exception("EventHandler ActionT is null,eventName=" + eventHandler.EventName));
                    return false;
                }

                if (null == eventHandler.Parameters || eventHandler.Parameters.Length == 0)
                {
                    eventHandler.Method.Invoke(eventHandler.Target,null);
                    return true;
                }
               
                for (var i = 0; i < eventHandler.Parameters.Length; i++)
                {
                    //TODO:这里的类型转换需要优化，目前类型不同直接放弃类型转换
                    if (null != args && args.Length > i && args[i] != null && eventHandler.Parameters[i].ParameterType == args[i].GetType())
                    {
                        fixArgs[i] = args[i];
                    }
                    else
                    {
                        if (eventHandler.Parameters[i].HasDefaultValue)
                        {
                            fixArgs[i] = eventHandler.Parameters[i].DefaultValue;
                        }
                        else
                        {
                            if (eventHandler.Parameters[i].ParameterType.IsValueType)
                            {
                                fixArgs[i] = Activator.CreateInstance(eventHandler.Parameters[i].ParameterType);
                            }
                            else
                            {
                                fixArgs[i] = null;
                            }
                            
                        }
                    }
                }

                if (eventHandler.RemainCount > 0)
                {
                    eventHandler.RemainCount--;
                }
                
                eventHandler.Method.Invoke(eventHandler.Target, fixArgs);
             
            }
            catch (Exception exception)
            {
                EventProcessCallback.OnError?.Invoke(eventHandler, args, exception);
                return false;
            }

            return true;
        }

        #endregion


#if UNITY_EDITOR
        #region Log

        // internal int DispatchCounter => DispatchSuccessCounter + DispatchFailCounter;
        // internal int DispatchSuccessCounter;
        // internal int DispatchFailCounter;
        // internal DateTime LastInvokeDateTime;
        // internal static List<EventLogData> Logs = new List<EventLogData>();
        //
        // internal static void CacheLog(EventHandler eventHandler, object[] args, bool success, Exception exception)
        // {
        //     if (success)
        //     {
        //         eventHandler.DispatchSuccessCounter++;
        //     }
        //     else
        //     {
        //         eventHandler.DispatchFailCounter++;
        //     }
        //
        //     eventHandler.LastInvokeDateTime = DateTime.Now;
        //
        //     var log = new EventLogData
        //     {
        //         EventName = eventHandler.EventName,
        //         DateTime = DateTime.Now,
        //         Success = success,
        //         Exception = exception,
        //     };
        //
        //     log.Parameters = "[";
        //     for (var i = 0; i < args.Length; i++)
        //     {
        //         var arg = args[i];
        //         log.Parameters += arg;
        //         if (i < args.Length - 1)
        //         {
        //             log.Parameters += ",";
        //         }
        //     }
        //
        //     log.Parameters += "]";
        //     Logs.Add(log);
        //
        //     if (Logs.Count > 1000)
        //     {
        //         Logs.RemoveAt(0);
        //     }
        // }

        #endregion

    

#endif
        public virtual void Reset()
        {
            Target = null;
            Interrupt = false;
            Parameters = null;
            Method = null;
            Priority = 0;
            RemainCount = -1;
            handlerId = -1;
        }

        public virtual void Dispose()
        {
            Target = null;
            Interrupt = false;
            Parameters = null;
            Method = null;
            Priority = 0;
            handlerId = -1;
        }
    }
}