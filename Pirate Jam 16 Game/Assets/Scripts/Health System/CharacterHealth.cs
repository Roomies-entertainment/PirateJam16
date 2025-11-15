using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class CharacterHealth : Health
{
    public bool blocking { get; private set; }

    [Header("")]
    [SerializeField] private UnityEvent onStartBlocking;
    [SerializeField] private UnityEvent onStopBlocking;

    public virtual void StartBlocking()
    {
        if (debug)
        {
            Debug.Log($"{this} is blocking");
        }

        blocking = true;

        onStartBlocking?.Invoke();
    }

    public virtual void StopBlocking()
    {
        if (debug)
        {
            Debug.Log($"{this} stopped blocking");
        }

        blocking = false;

        onStopBlocking?.Invoke();
    }

    protected override AttackResult ProcessDamageFlags(
        bool blockColliderHit, bool damageColliderHit, DetectionData data)
       
    {
        return base.ProcessDamageFlags(blockColliderHit || blocking, damageColliderHit, data);
    }
}
