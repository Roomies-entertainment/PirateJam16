using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RandomValue
{
    [SerializeField] protected float @default;
    [SerializeField] protected float alt;
    private float value;

    [Space()]
    [Tooltip("Range (random value from default to alt)\n\nSwitch (50% default, 50% alt)")]
    [SerializeField] protected RandomM.RandomType randomType;

    public float GetValue(bool reCalculate)
    {
        if (reCalculate)
        {

            switch (randomType)
            {
                case RandomM.RandomType.Range:
                    value = RandomM.Range(@default, alt); break;
                case RandomM.RandomType.Switch:
                    value = RandomM.Float0To1() <= 0.5f ? @default : alt; break;
                default:
                    value = @default; break;
            }
        }

        return value;
    }
}
