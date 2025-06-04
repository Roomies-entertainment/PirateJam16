using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class EventChain : MonoBehaviour
{
    [SerializeField] private bool loop;

    [Header("Events")]
    [SerializeField] private List<EventData> events;
    public EventData currentEvent { get { return events[index]; } }
    private int index = 0;

    private new bool enabled;
    public bool GetEnabled() { return enabled; }
    public void SetEnabled(bool setTo)
    {
        if (!enabled && setTo)
        {
            enabled = true;
            base.enabled = true;

            index = 0;
            events[index].StartEvent();
        }
        else if (enabled && !setTo)
        {
            enabled = false;
        }
    }

    private void OnEnable()
    {
        SetEnabled(true);
    }

    private void OnDisable()
    {
        if (!enabled)
        {
            return;
        }

        //Debug.Log($"{this} - Disabling queued");

        base.enabled = true;
        SetEnabled(false);
    }

    private void Update()
    {
        if (index >= events.Count)
        {
            return;
        }

        events[index].UpdateEvent(out bool eventCalled);

        if (eventCalled)
        {
            events[index].StartEvent();

            if (enabled && loop)
            {
                index = (index + 1) % events.Count;
            }
        }
        else
        {
            if (index >= events.Count)
            {
                enabled = false;
            }
        }
    }
}
