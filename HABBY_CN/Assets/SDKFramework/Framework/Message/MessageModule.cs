using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDKFramework.Message
{
    public class MsgType
    {
    }

    public class MessageModule : BaseModule
    {
        public delegate void MessageEventArgs<T>(T arg) where T : struct;

        /// <summary>
        /// 存放侦听者的集合
        /// </summary>
        public Dictionary<Type, List<Delegate>> msgMap;

        protected internal override void OnModuleInit()
        {
            base.OnModuleInit();
            msgMap = new Dictionary<Type, List<Delegate>>();
        }

        /// <summary>
        ///  注册 订阅  侦听，一个意思
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msgType"></param>
        /// <param name="Listener"></param>
        public void Subscribe<T>(MessageEventArgs<T> Listener) where T : struct
        {
            Type t = typeof(T);
            if (msgMap.TryGetValue(t, out var list)) //通过传过来的类型，找到管理委托的集合
            {
                if (!list.Contains(Listener)) //判断集合包含不包含这个委托
                {
                    list.Add(Listener); //不包含向集合里添加委托
                }
            }
            else //在字典没有找到存委托的集合
            {
                List<Delegate> lst = new List<Delegate>(); //创建一个委托集合
                lst.Add(Listener); //把委托添加到集合
                msgMap.Add(t, lst); //再用字典添加委托集合
            }
        }

        /// <summary>
        /// 移除注册、订阅、侦听，一个意思
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msgType"></param>
        /// <param name="Listener"></param>
        public void UnSubscribe<T>(T msgType, MessageEventArgs<T> Listener) where T : struct
        {
            Type t = msgType.GetType();
            if (msgMap.TryGetValue(t, out var list))
            {
                if (list.Contains(Listener))
                {
                    list.Remove(Listener);
                }
            }
            else
            {
                Debug.LogWarning("消息ID{t.name}没有注册,无法移除");
            }
        }

        /// <summary>
        /// 移除某个消息的全部订阅者，侦听者
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msgType"></param>
        public void UnSubscribe<T>(T msgType) where T : struct //重载移除函数字典里的集合委托
        {
            Type t = msgType.GetType();
            if (msgMap.TryGetValue(t, out var list))
            {
                list.Clear();
                msgMap.Remove(t);
            }
            else
            {
                Debug.LogWarning("消息ID{t.name}没有注册,无法移除");
            }
        }

        /// <summary>
        /// 向订阅者消息派发
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msgType"></param>
        public void Post<T>(T msgType) where T : struct
        {
            Type t = msgType.GetType();
            if (msgMap.TryGetValue(t, out var list))
            {
                foreach (MessageEventArgs<T> item in list)
                {
                    item?.Invoke(msgType);
                }
            }
            else
            {
                Debug.LogError($"消息ID{t}没有注册，无法派发");
            }
        }
    }
}