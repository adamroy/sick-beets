using UnityEngine;
using System.Collections.Generic;

public class EventSubscriber : MonoBehaviour
{
    public List<EventManager.Event> subscribedEvents;

    private void Awake ()
    {
	    foreach(var e in subscribedEvents)
        {
            EventManager.Subscribe(e, this.gameObject);
        }
	}
}
