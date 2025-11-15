using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class CharacterHealth : Health
{
    public bool startBlockFlag { get; protected set; }
    public bool blockFlag { get; protected set; }
    public bool stopBlockFlag { get; protected set; }

    [Header("")]
    [SerializeField] private UnityEvent onStartBlocking;
    [SerializeField] private UnityEvent onStopBlocking;

    public virtual void StartBlocking()
    {
        if (debug)
        {
            Debug.Log($"{this} is blockFlag");
        }

        startBlockFlag = true;
        blockFlag = true;

        onStartBlocking?.Invoke();
    }

    public virtual void StopBlocking()
    {
        if (debug)
        {
            Debug.Log($"{this} stopped blockFlag");
        }

        stopBlockFlag = true;
        blockFlag = false;

        onStopBlocking?.Invoke();
    }

    protected override AttackResult ProcessDamageFlags(
        bool blockColliderHit, bool damageColliderHit, DetectionData data)
       
    {
        return base.ProcessDamageFlags(blockColliderHit || blockFlag, damageColliderHit, data);
    }

    private void LateUpdate()
    {
        startBlockFlag = false;
        stopBlockFlag = false;
    }
}
