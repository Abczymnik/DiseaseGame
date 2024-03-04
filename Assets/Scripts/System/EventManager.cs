using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class TypedEvent : UnityEvent<object> { }

public class EventManager : MonoBehaviour
{
    private Dictionary<int, UnityEvent> eventDict;
    private Dictionary<int, TypedEvent> typedEventDict;

    private static EventManager eventManager;
    public static EventManager Instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindAnyObjectByType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("There is no active EventManager");
                }

                else eventManager.Init();
            }

            return eventManager;
        }
    }

    private void Init()
    {
        if (eventDict == null) eventDict = new Dictionary<int, UnityEvent>();
        if (typedEventDict == null) typedEventDict = new Dictionary<int, TypedEvent>();
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
        if (eventManager == null) return;

        if (Instance.eventDict.TryGetValue((int)eventName, out UnityEvent thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void StopListening(TypedEventName eventName, UnityAction<object> listener)
    {
        if (eventManager == null) return;

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