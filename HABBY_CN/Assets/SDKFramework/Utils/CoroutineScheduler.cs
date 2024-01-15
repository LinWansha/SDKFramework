using System;
using System.Collections;
using UnityEngine;

public class CoroutineScheduler : MonoBehaviour
{
    
    private static CoroutineScheduler _instance;

    public static CoroutineScheduler Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject timeManagerObj = new GameObject("CoroutineScheduler");
                _instance = timeManagerObj.AddComponent<CoroutineScheduler>();

                DontDestroyOnLoad(timeManagerObj);
            }

            return _instance;
        }
    }

    public Coroutine StartCoroutineCustom(IEnumerator enumerator)
    {
        return StartCoroutine(enumerator);
    }

    public void DrivingBehavior(Action action, float sec)
    {
        StartCoroutine(WaitAndExecute(action, sec));
    }

    private IEnumerator WaitAndExecute(Action action, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }

    public void DrivingBehavior<T>(Action<T> action, float sec)
    {
    }

    public void DrivingBehavior<T, K>(Action<T, K> action, float sec)
    {
    }

    public void DrivingBehavior<T, K, U>(Action<T, K, U> action, float sec)
    {
    }
}