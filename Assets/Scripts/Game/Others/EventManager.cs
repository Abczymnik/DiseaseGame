using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class TypedEvent : UnityEvent<object> { }

public class EventManager : MonoBehaviour
{
    private Dictionary<string, UnityEvent> eventDict;
    private Dictionary<string, TypedEvent> typedEventDict;

    private static EventManager eventManager;
    public static EventManager Instance
    {
        get
        {
            if(!eventManager)
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
        if (eventDict == null) eventDict = new Dictionary<string, UnityEvent>();
        if (typedEventDict == null) typedEventDict = new Dictionary<string, TypedEvent>();
    }

    public static void StartListening(string eventName, UnityAction listener)
    {
        if (Instance.eventDict.TryGetValue(eventName, out UnityEvent thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Instance.eventDict.Add(eventName, thisEvent);
        }
    }

    public static void StartListening(string eventName, UnityAction<object> listener)
    {
        if (Instance.typedEventDict.TryGetValue(eventName, out TypedEvent thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new TypedEvent();
            thisEvent.AddListener(listener);
            Instance.typedEventDict.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction listener)
    {
        if (eventManager == null) return;

        if (Instance.eventDict.TryGetValue(eventName, out UnityEvent thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void StopListening(string eventName, UnityAction<object> listener)
    {
        if (eventManager == null) return;

        if (Instance.typedEventDict.TryGetValue(eventName, out TypedEvent thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        if(Instance.eventDict.TryGetValue(eventName, out UnityEvent thisEvent))
        {
            thisEvent.Invoke();
        }
    }

    public static void TriggerEvent(string eventName, object data)
    {
        if (Instance.typedEventDict.TryGetValue(eventName, out TypedEvent thisEvent))
        {
            thisEvent.Invoke(data);
        }
    }
}
