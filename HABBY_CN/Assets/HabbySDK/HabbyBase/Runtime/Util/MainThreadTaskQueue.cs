using System;
using System.Collections.Generic;

namespace HabbySDK.HabbyTimerManager
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class MainThreadTaskQueue 
    {
        private static readonly Queue<Action> tasks = new Queue<Action>();
        private static readonly object queueLock = new object();

        public static void EnqueueTask(Action action)
        {
            lock (queueLock)
            {
                tasks.Enqueue(action);
            }
        }


        public static void ExecuteTasks()
        {
            if (tasks.Count > 0)
            {
                Queue<Action> localTasks;
                lock (queueLock)
                {
                    localTasks = new Queue<Action>(tasks);
                    tasks.Clear();
                }

                while (localTasks.Count > 0)
                {
                    var task = localTasks.Dequeue();
                    try
                    {
                        task?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning($"Error executing task: {ex}");
                    }
                }
            }
        }
    }
}