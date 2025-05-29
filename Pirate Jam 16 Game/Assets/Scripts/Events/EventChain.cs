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

    private new bool enabled;
    public bool GetEnabled() { return enabled; }
    public void SetEnabled(bool setTo)
    {
        if (!enabled && setTo)
        {
            enabled = true;
            base.enabled = true;

            index = 0;

            SetDelay();
            timer = 0f;
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
                    enabled = false;
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
