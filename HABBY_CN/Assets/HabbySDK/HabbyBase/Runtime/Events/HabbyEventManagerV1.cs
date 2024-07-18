using System;
using System.Collections.Generic;
using Habby.Base;
using UnityEngine;

namespace Habby.Events
{
    public class HabbyEventManagerV1:UnitySingletonBase<HabbyEventManagerV1>
    {
        public static bool LogError = false;
        
        #region Property

        /// <summary>
        /// 事件类型 - 事件分发器 字典
        /// </summary>
        internal static readonly Dictionary<string, EventHandlerGroup> DispatcherDic = new Dictionary<string, EventHandlerGroup>();

        private static readonly HabbyLinkListObjectPool<EventHandlerGroup> _objectPool = new HabbyLinkListObjectPool<EventHandlerGroup>();

        public static void ClearCache()
        {
            _objectPool.Clear();
            EventHandlerGroup.ClearCache();
        }
            
        #endregion
        
        #region Execute Unity Thread Update

        internal static bool NoUpdate = true;
        internal static List<Action> UpdateQueue = new List<Action>();
        internal static List<Action> UpdateRunQueue = new List<Action>();

        /// <summary>
        /// 附加到 Update 执行，用于保证线程安全情况下分发事件，但会延迟一帧
        /// </summary>
        /// <param name="action">action</param>
        internal void ExecuteUpdate(Action action)
        {
            lock (UpdateQueue)
            {
                UpdateQueue.Add(action);
                NoUpdate = false;
            }
            _objectPool.Clear();
        }

        #endregion

        #region MonoBehaviour

        private void Update()
        {
            lock (UpdateQueue)
            {
                if (NoUpdate) return;
                UpdateRunQueue.AddRange(UpdateQueue);
                UpdateQueue.Clear();
                NoUpdate = true;
                for (var i = 0; i < UpdateRunQueue.Count; i++)
                {
                    var action = UpdateRunQueue[i];
                    action?.Invoke();
                }

                UpdateRunQueue.Clear();
            }
            
        }

        #endregion


        #region dispatch
       // /// <summary>
       // /// 分发事件
       // /// </summary>
       // /// <param name="eventName">事件名称</param>
       // /// <param name="safeDispatch"></param>
       // /// <param name="args"></param>
       //  public void Dispatch(string eventName, bool safeDispatch = false,params object[] args)
       //  {
       //      if (HasEventListenr(eventName))
       //      {
       //          var eventHandlerGroup = _getOrAddHandlerGroup(eventName);
       //          eventHandlerGroup.Dispatch(safeDispatch,args);
       //      }
       //      else
       //      {
       //          EventProcessCallback.OnMissing?.Invoke(eventName);
       //      }
       //    
       //  }
       public void Dispatch(string eventName,params object[] args)
       {
           if (HasEventListenr(eventName))
           {
               var eventHandlerGroup = _getOrAddHandlerGroup(eventName);
               eventHandlerGroup.Dispatch(false,args);
           }
           else
           {
               EventProcessCallback.OnMissing?.Invoke(eventName);
           }
          
       }
       public void DispatchWithSafe(string eventName,params object[] args)
       {
           if (HasEventListenr(eventName))
           {
               var eventHandlerGroup = _getOrAddHandlerGroup(eventName);
               eventHandlerGroup.Dispatch(true,args);
           }
           else
           {
               EventProcessCallback.OnMissing?.Invoke(eventName);
           }
          
       }
       
        
        
        /// <summary>
        /// 获取事件分发器
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <returns>事件分发器</returns>
        public EventHandlerGroup _getOrAddHandlerGroup(string eventName)
        {
            if (!DispatcherDic.ContainsKey(eventName))
            {
                DispatcherDic[eventName] = _objectPool.Get();
                DispatcherDic[eventName].EventName = eventName;
            }
            return DispatcherDic[eventName];
        }

        public bool HasEventListenr(string eventName)
        {
            if (DispatcherDic.ContainsKey(eventName))
            {
                return DispatcherDic[eventName].Count > 0;
            }

            return false;
        }

        #endregion

        #region add listenre
        public long AddListener(string eventName,Action action,int remainCount = -1, int priority = 0, bool interrupt = false)
        {
            var eventHandlerGroup = _getOrAddHandlerGroup(eventName);
            return eventHandlerGroup.AddHandler(action,remainCount,priority,interrupt);
        }
        
        public long AddListener<T>(string eventName,Action<T> action,int remainCount = -1, int priority = 0, bool interrupt = false)
        {
            var eventHandlerGroup = _getOrAddHandlerGroup(eventName);
            return eventHandlerGroup.AddHandler( action,remainCount,priority,interrupt);
        }
        
        public long AddListener<T1,T2>(string eventName,Action<T1,T2> action,int remainCount = -1, int priority = 0, bool interrupt = false)
        {
            var eventHandlerGroup = _getOrAddHandlerGroup(eventName);
            return eventHandlerGroup.AddHandler( action,remainCount,priority,interrupt);
        }
        
        public long AddListener<T1,T2,T3>(string eventName,Action<T1,T2,T3> action,int remainCount = -1, int priority = 0, bool interrupt = false)
        {
            var eventHandlerGroup = _getOrAddHandlerGroup(eventName);
            return eventHandlerGroup.AddHandler( action,remainCount,priority,interrupt);
        }


        #endregion
        #region Remove Listener
        public bool RemoveLisenerById(string eventName, long id)
        {
            if (HasEventListenr(eventName))
            {
                var eventHandlerGroup = _getOrAddHandlerGroup(eventName);
                bool result = eventHandlerGroup.RemoveById(id);
                if (eventHandlerGroup.Count == 0)
                {
                    DispatcherDic.Remove(eventName);
                    _objectPool.Return(eventHandlerGroup);
                }
                return result;
            }

            return false;
        }
        /// <summary>
        /// 移除监听
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="action">监听委托</param>
        public bool RemoveListener(string eventName,Action action)
        {
            if (HasEventListenr(eventName))
            {
                var eventHandlerGroup = _getOrAddHandlerGroup(eventName);
                bool result = eventHandlerGroup.RemoveByAction(action);
                if (eventHandlerGroup.Count == 0)
                {
                    DispatcherDic.Remove(eventName);
                    _objectPool.Return(eventHandlerGroup);
                }
                return result;
            }

            return false;
        }
        
        
        public bool RemoveListener<T>(string eventName,Action<T> action)
        {
            if (HasEventListenr(eventName))
            {
                var eventHandlerGroup = _getOrAddHandlerGroup(eventName);
                bool result = eventHandlerGroup.RemoveByAction(action);
                if (eventHandlerGroup.Count == 0)
                {
                    DispatcherDic.Remove(eventName);
                    _objectPool.Return(eventHandlerGroup);
                }
                return result;
            }

            return false;
        }
        
        public bool RemoveListener<T1,T2>(string eventName,Action<T1,T2> action)
        {
            if (HasEventListenr(eventName))
            {
                var eventHandlerGroup = _getOrAddHandlerGroup(eventName);
                bool result = eventHandlerGroup.RemoveByAction(action);
                if (eventHandlerGroup.Count == 0)
                {
                    DispatcherDic.Remove(eventName);
                    _objectPool.Return(eventHandlerGroup);
                }
                return result;
            }

            return false;
        }
        
        public bool RemoveListener<T1,T2,T3>(string eventName,Action<T1,T2,T3> action)
        {
            if (HasEventListenr(eventName))
            {
                var eventHandlerGroup = _getOrAddHandlerGroup(eventName);
                bool result = eventHandlerGroup.RemoveByAction(action);
                if (eventHandlerGroup.Count == 0)
                {
                    DispatcherDic.Remove(eventName);
                    _objectPool.Return(eventHandlerGroup);
                }
                return result;
            }

            return false;
        }
        
        public bool RemoveAllEventNameListener(string eventName)
        {
            if (HasEventListenr(eventName))
            {
                var eventHandlerGroup = _getOrAddHandlerGroup(eventName);
                eventHandlerGroup.Reset();
                DispatcherDic.Remove(eventName);
                _objectPool.Return(eventHandlerGroup);
                return true;
            }

            return false;
        }

        public void RemoveAll()
        {
            foreach (KeyValuePair<string,EventHandlerGroup> eventHandlerGroup in DispatcherDic)
            {
                eventHandlerGroup.Value.Dispose();
                _objectPool.Return(eventHandlerGroup.Value);
            }
            DispatcherDic.Clear();
        }
        
        #endregion

        public void DebugPrint()
        {
            Debug.Log($"============== events info start ==============");
            foreach (KeyValuePair<string,EventHandlerGroup> eventHandlerGroup in DispatcherDic)
            {
                Debug.Log($"### event={ eventHandlerGroup.Value.EventName},count={ eventHandlerGroup.Value.Count}");
            }
            Debug.Log($"============== events info end==============");
        }
        
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        protected static void Init()
        {
            var ins = Instance;
            EventProcessCallback.OnError += (eventHandler,  args, exception) =>
            {
                if (LogError)
                {
                    Debug.LogError(exception.ToString());
                }
                else
                {
                    Debug.Log($"---### OnDispatched error eventName={eventHandler.EventName},msg={exception}");
                }
               
            };
#if UNITY_EDITOR
            EventProcessCallback.OnDispatched += (eventHandler,args) =>
            {
                Debug.Log($"---### OnDispatched eventName={eventHandler.EventName}");
            };
            EventProcessCallback.OnAdded += (eventHandler) =>
            {
                Debug.Log($"---### add eventName={eventHandler.EventName} count={Instance._getOrAddHandlerGroup(eventHandler.EventName).Count}");
            };
            EventProcessCallback.OnRemoved += (eventHandler) =>
            {
                Debug.Log($"---### OnRemoved eventName={eventHandler.EventName}");
            };
            EventProcessCallback.OnMissing += (eventName) =>
            {
                Debug.Log($"---### OnMiss eventName={eventName}");
            };
#endif
            
        }
        
    }
    
    
}