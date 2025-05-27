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
     
    private EventData currentEvent;
    private int index = 0;
    private float delay;
    private float timer;

    private bool _enabled;
    public new bool enabled
    {
        get { return _enabled; }
        set { if (!_enabled && value) { DoOnEnable(); } else if (_enabled && !value) { DoOnDisable(); } }
    }

    private void OnEnable()
    {
        if (!_enabled)
        {
            DoOnEnable();
        }
    }

    private void DoOnEnable()
    {
        _enabled = true;
        base.enabled = true;

        index = 0;

        SetDelay();
        timer = 0f;
    }

    private void DoOnDisable()
    {
        _enabled = false;
    }

    private void Update()
    {
        if (index >= events.Count)
        {
            return;
        }

        if (timer >= delay)
        {
            currentEvent = events[index];
            currentEvent.Event?.Invoke();

            if (enabled && loop)
            {
                index = (index + 1) % events.Count;
            }
            else
            {
                index = index + 1;

                if (index < events.Count)
                {
                    SetDelay();
                    timer = 0f;
                }
                else
                {
                    base.enabled = false;
                }
            }
        }

        timer += Time.deltaTime;
    }

    private void SetDelay()
    {
        if (index >= events.Count)
            return;

        delay = events[index].GetDelay();
    }
}
