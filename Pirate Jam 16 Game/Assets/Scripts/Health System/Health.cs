using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Health : MonoBehaviour, IProcessExplosion
{
    [SerializeField]
    [Tooltip("Object starts dead if this is 0")]
    protected int startingHealth = 1;

    [SerializeField]
    protected int maxHealth = 1;

    [Header("")]
    [SerializeField] [Tooltip("Delay for OnDie events assigned in inspector")]
    protected DelayRandomized onDieDelay = new();
    [SerializeField] [Tooltip("Delay for OnDie events assigned in inspector when dying from explosion")]
    protected DelayRandomized explosionOnDieDelay = new();

    [SerializeField] [Tooltip(  "Used any time this component's DestroyObject() method destroys itself or another object\n" +
                                "Use DestroyObjectNoDelay to ignore")]
    protected DelayRandomized destroyObjectDelay = new();
    
    public int health { get; protected set; }
    public bool dead { get { return health <= 0; } }

    [Header("")]
    [SerializeField] protected bool damagedByExplosions = true;
    [Tooltip("Don't block attacks from behind")]
    [SerializeField] protected bool blockBehindCheck = true;

    [Header("")]
    private Vector2 blockDirection;
    public void SetBlockDirection(Vector2 setTo) { blockDirection = setTo; }
    [SerializeField] protected float blockBehindCheckLeniance = -0.3f;

    [Header("")]
    [SerializeField] protected Collider2D[] TakeDamageColliders;
    [SerializeField] protected Collider2D[] BlockDamageColliders;

    [Header("")]
    [SerializeField] private UnityEvent<float, DetectionData> onTakeDamage;
    [SerializeField] private UnityEvent<float, DetectionData> onHeal;
    [SerializeField] private UnityEvent<float, DetectionData> onBlockDamage;
    [SerializeField] private UnityEvent<DetectionData> onDie;

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


    public void ProcessExplosion(Explosion explosion)
    {
        if (damagedByExplosions)
        {
            if (debug)
            {
                Debug.Log($"{gameObject.name} taking explosion damage");
            }

            var data = new DetectionData(explosion.transform.position, this, explosion);

            IncrementHealth(-explosion.damage, data);
        }
    }

    public virtual void IncrementHealth(int increment, DetectionData data)
    {
        if (!enabled)
        {
            return;
        }

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
        {
            if (explosionOnDieDelay.GetDelay(true) > 0 && data.DetectorComponent.GetType().IsAssignableFrom(typeof(Explosion)))
            {
                StartCoroutine(OnDieDelayed(data, explosionOnDieDelay.GetDelay(false)));
            }

            else if (onDieDelay.GetDelay(true) > 0)
            {
                StartCoroutine(OnDieDelayed(data, onDieDelay.GetDelay(false)));
            }

            else
            {
                OnDie(data);
            }
        }
            
    }

    private IEnumerator OnDieDelayed(DetectionData data, float delay)
    {
        yield return new WaitForSeconds(delay);

        OnDie(data);
    }

    protected virtual void OnDie(DetectionData data)
    {
        onDie?.Invoke(data);
    }

    public new void DestroyObject(Object objOverride = null)
    {
        Destroy(objOverride != null ? objOverride : gameObject, destroyObjectDelay.GetDelay(true));
    }

    public void DestroyObjectNoDelay(Object objOverride = null)
    {
        Destroy(objOverride != null ? objOverride : gameObject);
    }

    public virtual AttackResult ProcessAttack(int damage, DetectionData data)
    {
        if (!enabled)
        {
            return AttackResult.Miss;
        }

        AttackResult attackResult = ProcessDamageFlags(
            BlockDamageColliderHit(data),
            TakeDamageColliderHit(data),
            data);

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

    protected bool BlockDamageColliderHit(DetectionData data)
    {
        foreach (var c in BlockDamageColliders)
        {
            if (data.detectedColliders.Contains(c))
            {
                return true;
            }
        }

        return false;
    }

    protected bool TakeDamageColliderHit(DetectionData data)
    {
        foreach (var c in TakeDamageColliders)
        {
            if (data.detectedColliders.Contains(c))
            {
                return true;
            }
        }

        return false;
    }

    protected virtual AttackResult ProcessDamageFlags(
        bool blockColliderHit, bool damageColliderHit, DetectionData data)
       
    {
        if (blockColliderHit && (
            !blockBehindCheck || Detection.DirectionCheck(
                blockDirection, transform.position, data.DetectorComponent.transform.position,
                false, blockBehindCheckLeniance)))
        {
            return AttackResult.Block;
        }

        if (damageColliderHit)
        {
            return AttackResult.Hit;
        }

        return AttackResult.Miss;
    }

    protected virtual void BlockDamage(int damage, DetectionData data)
    {
        if (debug)
        {
            Debug.Log($"{gameObject.name} blocked {damage} damage");
        }

        onBlockDamage?.Invoke(damage, data);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
