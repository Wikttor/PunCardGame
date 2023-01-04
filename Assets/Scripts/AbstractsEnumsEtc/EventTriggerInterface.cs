using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventTriggerInterface : EventTrigger
{
    public enum supportedEvents
    {
        OnMouseZeroDown,
        OnMouseOneDown,
    }

    private Dictionary<supportedEvents, Action> eventsDict;

    public  void AddEventTrigger(supportedEvents triggerType, Action action)
    {
        if(eventsDict == null)
        {
            eventsDict = new Dictionary<supportedEvents, Action>();
        }

        if (!eventsDict.ContainsKey(triggerType))
        {
            eventsDict.Add(triggerType, action);
        }
        else
        {
            eventsDict[triggerType] = eventsDict[triggerType] + action;
        }
    }

    public static void AddEventTriggerStatic(supportedEvents triggerType, Action action, GameObject triggeringObject)
    {
        EventTriggerInterface component = triggeringObject.GetComponent<EventTriggerInterface>();
        if(component == null)
        {
            component = triggeringObject.AddComponent<EventTriggerInterface>();
        }       
        component.AddEventTrigger(triggerType, action);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (Input.GetMouseButtonDown(0))
        {
            if (eventsDict.ContainsKey(supportedEvents.OnMouseZeroDown))
            {
                eventsDict[supportedEvents.OnMouseZeroDown]();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (eventsDict.ContainsKey(supportedEvents.OnMouseOneDown))
            {
                eventsDict[supportedEvents.OnMouseOneDown]();
            }
        }
    }
}