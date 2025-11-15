using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class EventData
{
    [SerializeField] private UnityEvent Event;
    [SerializeField] private DelayRandomized delayComponent;
    private float timer;
    private float currentDelay;

    public EventData StartEvent()
    {
        Reset();

        return this;
    }

    public void UpdateEvent(out bool eventCalled)
    {
        if (timer >= currentDelay)
        {
            Event?.Invoke();
            eventCalled = true;
            Reset();
        }
        else
        {
            eventCalled = false;
        }

        timer += Time.deltaTime;
    }

    private void Reset()
    {
        currentDelay = delayComponent.GetDelay(true);
        timer = 0f;
    }
}

public class Event : MonoBehaviour
{
    public bool loop;
    [SerializeField] private EventData _event;

    private void OnEnable()
    {
        _event.StartEvent();
    }

    private void Update()
    {
        _event.UpdateEvent(out bool eventCalled);

        if (eventCalled)
        {
            if (loop)
            {
                _event.StartEvent();
            }
            else
            {
                enabled = false;
            }
        }
    }
}