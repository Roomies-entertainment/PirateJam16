using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Health : MonoBehaviour, IProcessExplosion
{
    [SerializeField] [Tooltip("Object starts dead if this is 0")]
    protected int startingHealth = 1;

    [SerializeField]
    protected int maxHealth = 1;
    
    [Header("")]
    [SerializeField][Tooltip("Used for DestroyObject()")]
    protected float destroyObjectDelay = 0.8f;
    public int health { get; protected set; }
    public bool dead { get { return health <= 0; } }

    [Header("")]
    [SerializeField] protected bool damagedByExplosions = true;
    [SerializeField] protected bool blockDirectionChecking = true;
    [SerializeField] protected float blockDirectionCheckDistance = -0.3f;

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

    private Vector2 blockDirection;
    public void SetBlockDirection(Vector2 setTo)
    {
        blockDirection = setTo;
    }

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

    protected virtual void Start() { } // Gives it enabled checkbox

    public void IncrementHealth(int increment)
    {
        if (!enabled)
        {
            return;
        }

        IncrementHealth(increment, null);
    }

    public void ProcessExplosion()
    {
        if (damagedByExplosions)
            IncrementHealth(-1);
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

    public new void DestroyObject(Object objOverride = null)
    { 
        Destroy(objOverride != null ? objOverride : gameObject, destroyObjectDelay);
    }
    
    public void DestroyObjectNoDelay(Object objOverride = null)
    { 
        Destroy(objOverride != null ? objOverride : gameObject);
    }

    public virtual AttackResult ProcessAttack(int damage, DetectionData<Health, Attack> data)
    {
        if (!enabled)
        {
            return AttackResult.Miss;
        }

        bool blockColliderHit = BlockDamageColliderHit(data);
        bool damageColliderHit = TakeDamageColliderHit(data);

        AttackResult attackResult = ProcessDamageFlags(blocking, blockColliderHit, damageColliderHit, data);

        switch (attackResult)
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

    protected virtual AttackResult ProcessDamageFlags(
        bool blocking, bool blockColliderHit, bool damageColliderHit, DetectionData<Health, Attack> data)
    {
        if (( blocking || blockColliderHit ) &&
            !blockDirectionChecking || Detection.DirectionCheck(
                blockDirection, transform.position, data.DetectorComponent.Component.transform.position,
                blockDirectionCheckDistance))
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
