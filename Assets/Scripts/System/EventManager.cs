using System;
using System.Collections.Generic;
using UnityEngine.Events;

[Serializable]
public class TypedEvent : UnityEvent<object> { }

public sealed class EventManager
{
    private static readonly object instanceLock = new object();

    private Dictionary<int, UnityEvent> eventDict;
    private Dictionary<int, TypedEvent> typedEventDict;

    private static EventManager _instance;
    public static EventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new EventManager();

                        _instance.InitializeEventsBasedManagers();
                    }
                }
            }
            return _instance;
        }
    }

    private EventManager()
    {
        eventDict = new Dictionary<int, UnityEvent>();
        typedEventDict = new Dictionary<int, TypedEvent>();
    }

    private void InitializeEventsBasedManagers()
    {
        EnvironmentManager.Instance.Init();
    }

    public static void StartListening(UnityEventName eventName, UnityAction listener)
    {
        if (Instance.eventDict.TryGetValue((int)eventName, out UnityEvent thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Instance.eventDict.Add((int)eventName, thisEvent);
        }
    }

    public static void StartListening(TypedEventName eventName, UnityAction<object> listener)
    {
        if (Instance.typedEventDict.TryGetValue((int)eventName, out TypedEvent thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new TypedEvent();
            thisEvent.AddListener(listener);
            Instance.typedEventDict.Add((int)eventName, thisEvent);
        }
    }

    public static void StopListening(UnityEventName eventName, UnityAction listener)
    {
        if (_instance == null) return;

        if (Instance.eventDict.TryGetValue((int)eventName, out UnityEvent thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void StopListening(TypedEventName eventName, UnityAction<object> listener)
    {
        if (_instance == null) return;

        if (Instance.typedEventDict.TryGetValue((int)eventName, out TypedEvent thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(UnityEventName eventName)
    {
        if (Instance.eventDict.TryGetValue((int)eventName, out UnityEvent thisEvent))
        {
            thisEvent.Invoke();
        }
    }

    public static void TriggerEvent(TypedEventName eventName, object data)
    {
        if (Instance.typedEventDict.TryGetValue((int)eventName, out TypedEvent thisEvent))
        {
            thisEvent.Invoke(data);
        }
    }
}