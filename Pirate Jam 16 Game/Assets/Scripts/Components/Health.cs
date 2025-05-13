using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Health : MonoBehaviour
{
    [SerializeField] protected int startingHealth = 3;
    public int health { get; protected set; }

    [Header("")]
    [SerializeField] protected Collider2D[] TakeDamageColliders;
    [SerializeField] protected Collider2D[] BlockDamageColliders;

    [Header("")]
    [SerializeField] private UnityEvent<float, DetectionData> onTakeDamage;
    [SerializeField] private UnityEvent onStartBlocking;
    [SerializeField] private UnityEvent onStopBlocking;
    [SerializeField] private UnityEvent<float, DetectionData> onBlockDamage;
    [SerializeField] private UnityEvent onDie;

    public bool blocking { get; private set; }

    public enum AttackResult
    {
        Hit,
        Miss,
        Block
    }

    [Header("")]
    [SerializeField] protected bool debug;

    protected void Awake()
    {
        health = startingHealth;
    }

    protected virtual void IncrementHealth(int increment)
    {
        health = (int) Mathf.Max(0f, health + increment);

        if (health == 0)
            OnDie();
    }

    protected virtual void OnDie()
    {
        onDie?.Invoke();
    }

    public virtual AttackResult ProcessAttack(int damage, DetectionData data)
    {
        bool blockColliderHit = BlockDamageColliderHit(data);
        bool damageColliderHit = TakeDamageColliderHit(data);

        AttackResult attackResult = ProcessDamageFlags(blocking, blockColliderHit, damageColliderHit);

        switch(attackResult)
        {
            case AttackResult.Hit:
                ApplyDamage(damage, data);
                break;

            case AttackResult.Miss:
                break;
                
            case AttackResult.Block:
                BlockDamage(damage, data);
                break;
        }

        return attackResult;
    }

    protected bool BlockDamageColliderHit(DetectionData data)
    {
        foreach(var c in BlockDamageColliders)
        {
            if (data.DetectedComponent.Colliders.Contains(c))
            {
                return true;
            }
        }
        
        return false;
    }

    protected bool TakeDamageColliderHit(DetectionData data)
    {
        foreach(var c in TakeDamageColliders)
        {
            if (data.DetectedComponent.Colliders.Contains(c))
            {
                return true;
            }
        }
        
        return false;
    }

    protected virtual AttackResult ProcessDamageFlags(bool blocking, bool blockColliderHit, bool damageColliderHit)
    {
        if (blocking || blockColliderHit)
        {
            return AttackResult.Block;
        }

        if (damageColliderHit)
        {
            return AttackResult.Hit;
        }
        
        return AttackResult.Miss;
    }

    public virtual void ApplyDamage(int damage, DetectionData data)
    {
        if (debug)
        {
            Debug.Log($"{gameObject.name} took {damage} damage");
        }

        IncrementHealth(-damage);

        onTakeDamage?.Invoke((float) health / startingHealth, data);
    }

    protected virtual void BlockDamage(int damage, DetectionData data)
    {
        if (debug)
        {
            Debug.Log($"{gameObject.name} blocked {damage} damage");
        }

        onBlockDamage?.Invoke(damage, data);
    }

    public virtual void StartBlocking()
    {
        if (debug)
        {
            Debug.Log($"{gameObject.name} is blocking");
        }

        blocking = true;

        onStartBlocking?.Invoke();
    }

    public virtual void StopBlocking()
    {
        if (debug)
        {
            Debug.Log($"{gameObject.name} stopped blocking");
        }

        blocking = false;

        onStopBlocking?.Invoke();
    }
}
