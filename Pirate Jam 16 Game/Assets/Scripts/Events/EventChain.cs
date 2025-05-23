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
        set { if (!_enabled && value) { OnEnable(); } _enabled = value; }
    }

    private void Awake()
    {
        _enabled = base.enabled;
    }

    private void OnEnable()
    {
        _enabled = true;

        index = 0;

        SetDelay();

        timer = 0f;
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

            index = (enabled && loop) ? (index + 1) % events.Count : index + 1;

            SetDelay();
            timer = 0f;
        }

        timer += Time.deltaTime;
    }

    private void SetDelay()
    {
        if (index >= events.Count)
            return;

        delay = RandomM.Range(events[index].delayMin, events[index].delayMax);
    }
}
