using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace SDKFramework.Utils
{
    public class AsyncScheduler : MonoSingleton<AsyncScheduler>
    {
        // if u want wait a coroutine,can use this
        public static Task Yield(IEnumerator enumerator)
        {
            return new CoroutineAwaiter(enumerator).Task;
        }

        public void DelayedInvoke(Action action, float sec)
        {
            IEnumerator WaitAndExecute(Action a, float seconds)
            {
                yield return new WaitForSeconds(seconds);
                a?.Invoke();
            }

            StartCoroutine(WaitAndExecute(action, sec));
        }

        public static TaskCompletionSource<T> CreateTCS<T>()
        {
            var tcs = new TaskCompletionSource<T>();
            return tcs;
        }
    }

    internal class CoroutineAwaiter
    {
        private readonly TaskCompletionSource<object> TCS = AsyncScheduler.CreateTCS<object>();

        public CoroutineAwaiter(IEnumerator enumerator)
        {
            AsyncScheduler.Instance.StartCoroutine(WaitCoroutine(TCS, enumerator));
        }

        public Task Task => TCS.Task;

        private IEnumerator WaitCoroutine(TaskCompletionSource<object> tcs, IEnumerator enumerator)
        {
            yield return enumerator;
            tcs.SetResult(null);
        }
    }
}