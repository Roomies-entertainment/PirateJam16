using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class EventData
{
    public UnityEvent Event;
    public float minDelay;
    public float maxDelay;

    [Space()]
    [Tooltip("Randomized delay toggle")]
    public bool useMaxDelay = false;

    public float GetDelay()
    {
        return useMaxDelay ? RandomM.Range(minDelay, maxDelay) : minDelay;
    }
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
            timer = 0f;
        }

        timer += Time.deltaTime;
    }
}
