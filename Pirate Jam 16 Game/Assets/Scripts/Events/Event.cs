using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class EventData
{
    public UnityEvent Event;
    public DelayRandomized delay;
}

public class Event : MonoBehaviour
{
    [SerializeField] private bool loop;

    [SerializeField] private EventData _event;
    
    private float delay;
    private float timer;

    private void OnEnable()
    {
        timer = 0f;
    }

    private void Update()
    {
        if (timer >= delay)
        {
            _event.Event?.Invoke();
            delay = _event.delay.GetDelay(true);
            timer = 0f;
        }

        timer += Time.deltaTime;
    }
}
