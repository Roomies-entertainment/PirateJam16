using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class EventData
{
    public UnityEvent Event;
    public float delay;
    public float delayAlt;

    [Space()] [Tooltip("Range (random value from delay to delayAlt)\n\nSwitch (50% delay, 50% delayAlt)")]
    public RandomM.RandomType delayRandomType;

    public float GetDelay()
    {
        switch (delayRandomType)
        {
            case RandomM.RandomType.Range:
                return RandomM.Range(delay, delayAlt);
            case RandomM.RandomType.Switch:
                return RandomM.Float0To1() <= 0.5f ? delay : delayAlt;
            default:
                return delay;
        }
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
