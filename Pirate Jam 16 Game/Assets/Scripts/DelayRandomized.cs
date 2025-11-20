using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DelayRandomized
{
    [SerializeField] protected float _delay;
    [SerializeField] protected float _delayAlt;
    private float currentDelay;

    [Space()]
    [Tooltip("Range (random value from delay to delayAlt)\n\nSwitch (50% delay, 50% delayAlt)")]
    [SerializeField] protected RandomM.RandomType delayRandomType;

    public float GetDelay(bool reCalculate)
    {
        if (reCalculate)
        {

            switch (delayRandomType)
            {
                case RandomM.RandomType.Range:
                    currentDelay = RandomM.Range(_delay, _delayAlt); break;
                case RandomM.RandomType.Switch:
                    currentDelay = RandomM.Float0To1() <= 0.5f ? _delay : _delayAlt; break;
                default:
                    currentDelay = _delay; break;
            }
        }

        return currentDelay;
    }
}
