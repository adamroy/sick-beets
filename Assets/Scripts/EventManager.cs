using UnityEngine;
using System;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    public enum Event
    {
        UpdateSimulation,
        NeedsMet
    }

    static private EventManager instance;
    private Dictionary<Event, List<GameObject>> objectMap;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
        }

        objectMap = new Dictionary<Event, List<GameObject>>();

        foreach (var e in (Event[])Enum.GetValues(typeof(Event)))
        {
            objectMap.Add(e, new List<GameObject>());
        }
    }

    public static void Subscribe(Event e, GameObject go)
    {
        instance.objectMap[e].Add(go);
    }

    public static void UnSubscribe(Event e, GameObject go)
    {
        instance.objectMap[e].Remove(go);
    }

    public static void Broadcast(Event e, object argument)
    {
        if (instance == null) return;

        foreach(var go in instance.objectMap[e])
        {
            go.SendMessage(e.ToString(), argument);
        }
    }
}
