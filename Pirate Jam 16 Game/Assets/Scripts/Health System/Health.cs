using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Health : MonoBehaviour
{
    [SerializeField] [Tooltip("Object always considered to be dead if health is at 0")]
    protected int startingHealth = 1;

    [SerializeField]
    protected int maxHealth = 1;
    public int health { get; protected set; }
    public bool dead { get { return health <= 0; } }

    [Header("")]
    [SerializeField] protected Collider2D[] TakeDamageColliders;
    [SerializeField] protected Collider2D[] BlockDamageColliders;

    [Header("")]
    [SerializeField] private UnityEvent<float, DetectionData<Health, Attack>> onTakeDamage;
    [SerializeField] private UnityEvent<float, DetectionData<Health, Attack>> onHeal;
    [SerializeField] private UnityEvent onStartBlocking;
    [SerializeField] private UnityEvent onStopBlocking;
    [SerializeField] private UnityEvent<float, DetectionData<Health, Attack>> onBlockDamage;
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

    public void IncrementHealth(int increment)
    {
        if (!enabled)
        {
            return;
        }
        
        IncrementHealth(increment, null);
    }
    
    protected virtual void IncrementHealth(int increment, DetectionData<Health, Attack> data)
    {
        bool deadStore = dead;

        health = Mathf.Clamp(health + increment, 0, maxHealth);

        if (increment < 0)
        {
            if (debug)
            {
                Debug.Log($"{gameObject.name} took {-increment} damage");
            }

            onTakeDamage?.Invoke((float)health / maxHealth, data);
        }
        else if (increment > 0)
        {
            if (debug)
            {
                Debug.Log($"{gameObject.name} healed by {increment} points");
            }

            onHeal?.Invoke((float)health / maxHealth, data);
        }

        if (!deadStore && dead)
            OnDie();
    }

    protected virtual void OnDie()
    {
        onDie?.Invoke();
    }

    public void DestroyObject(GameObject destroy = null)
    {
        new WaitForSeconds(0.8f);

        if (destroy != null)
            Destroy(destroy);
        else
            Destroy(gameObject);
    }

    public virtual AttackResult ProcessAttack(int damage, DetectionData<Health, Attack> data)
    {
        if (!enabled)
        {
            return AttackResult.Miss;
        }

        bool blockColliderHit = BlockDamageColliderHit(data);
        bool damageColliderHit = TakeDamageColliderHit(data);

        AttackResult attackResult = ProcessDamageFlags(blocking, blockColliderHit, damageColliderHit);

        switch(attackResult)
        {
            case AttackResult.Hit:
                IncrementHealth(-damage, data);
                break;

            case AttackResult.Miss:
                break;
                
            case AttackResult.Block:
                BlockDamage(damage, data);
                break;
        }

        return attackResult;
    }

    protected bool BlockDamageColliderHit(DetectionData<Health, Attack> data)
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

    protected bool TakeDamageColliderHit(DetectionData<Health, Attack> data)
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

    protected virtual void BlockDamage(int damage, DetectionData<Health, Attack> data)
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
