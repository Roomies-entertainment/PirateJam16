using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DelayRandomized
{
    [SerializeField] protected float _delay;
    [SerializeField] protected float _delayAlt;
    private float delay;

    [Space()]
    [Tooltip("Range (random value from delay to delayAlt)\n\nSwitch (50% delay, 50% delayAlt)")]
    [SerializeField] protected RandomM.RandomType delayRandomType;

//    private bool calculate = true;

/* 
    public void QueueCalculateDelay()
    {
        calculate = true;
    }
 */
    public float GetDelay()
    {
/*         
        if (calculate)
        {
 */
            switch (delayRandomType)
            {
                case RandomM.RandomType.Range:
                    delay = RandomM.Range(_delay, _delayAlt); break;
                case RandomM.RandomType.Switch:
                    delay = RandomM.Float0To1() <= 0.5f ? _delay : _delayAlt; break;
                default:
                    delay = _delay; break;
            }
/* 
            calculate = false;
        }
*/
            return delay;
        }
}
