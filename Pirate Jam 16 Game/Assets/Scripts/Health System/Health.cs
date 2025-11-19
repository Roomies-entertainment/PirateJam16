using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Health : MonoBehaviour, IProcessExplosion, IProcessProjectile
{
    [SerializeField]
    [Tooltip("Object starts dead if this is 0")]
    protected int startingHealth = 1;

    public int maxHealth = 1;

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
    public bool dieFlag { get; protected set; }

    [Header("")]
    [SerializeField] protected bool damagedByExplosions = true;
    [SerializeField] protected bool damagedByProjectiles = true;
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
    // [SerializeField] private UnityEvent<float, DetectionData> onMissDamage;

    public void ProcessHealthEvents()
    {
        foreach (DamageEvent d in damageEvents)
            IncrementHealth(-d.amount, d.detectionData);

        foreach (DamageEvent h in healEvents)
            IncrementHealth(h.amount, h.detectionData);
        
        CheckIsDead();
    }

    public bool healthChangeFlag { get; private set; }
    public bool explosionDamageFlag { get; private set; }

    [SerializeField] private UnityEvent onDie;

    public List<DamageEvent> damageEvents = new();
    public List<DamageEvent> healEvents = new();

    public enum AttackResult
    {
        Hit,
        Miss,
        Block
    }

    [Header("")]
    [SerializeField] protected bool debug;

    protected void OnEnable()
    {
        health = startingHealth;
    }

    protected virtual void Start() { } // Gives it enabled checkbox

    public virtual AttackResult ProcessDamage(int damage, DetectionData data)
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
                damageEvents.Add(new DamageEvent(damage, data));
                break;

            case AttackResult.Miss:
            case AttackResult.Block:
                BlockDamage(damage, data);
                break;
        }

        return attackResult;
    }

    public virtual void ProcessHeal(int amount, DetectionData data)
    {
        healEvents.Add(new HealEvent(amount, data));
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

    public void ProcessExplosion(Explosion e)
    {
        if (!enabled || !damagedByExplosions)
        {
            return;
        }

        if (debug)
        {
            Debug.Log($"{this} taking explosion damage");
        }

        var data = new DetectionData(e.transform.position, e);
        damageEvents.Add(new DamageEvent(e.damage, data));
    }

    public virtual void ProcessProjectile(Projectile p)
    {
        if (!enabled || !damagedByProjectiles)
        {
            return;
        }

        if (debug)
        {
            Debug.Log($"{this} taking projectile damage");
        }

        var data = new DetectionData(p.transform.position, p);
        damageEvents.Add(new DamageEvent(p.damage, data));
    }

    

    public virtual void IncrementHealth(int increment, DetectionData data)
    {
        if (!enabled || increment == 0)
        {
            return;
        }        

        health = Mathf.Clamp(health + increment, 0, maxHealth);

        if (increment < 0)
        {
            if (debug)
            {
                Debug.Log($"{this} took {-increment} damage");
            }

            if (!explosionDamageFlag)
                explosionDamageFlag = data.DetectedBy.GetType().IsAssignableFrom(typeof(Explosion));
                
            onTakeDamage?.Invoke((float)health / maxHealth, data);
        }
        else if (increment > 0)
        {
            if (debug)
            {
                Debug.Log($"{this} healed by {increment} points");
            }

            onHeal?.Invoke((float)health / maxHealth, data);
        }

        healthChangeFlag = true;
    }

    public virtual void CheckIsDead()
    {
        if (dead)
        {
            if (explosionOnDieDelay.GetDelay(true) > 0 && explosionDamageFlag)
            {
                StartCoroutine(OnDieDelayed(explosionOnDieDelay.GetDelay(false)));
            }

            else if (onDieDelay.GetDelay(true) > 0)
            {
                StartCoroutine(OnDieDelayed(onDieDelay.GetDelay(false)));
            }

            else
            {
                OnDie();
            }
        }
    }

    private IEnumerator OnDieDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        OnDie();
    }

    protected virtual void OnDie()
    {
        dieFlag = true;
        onDie?.Invoke();
    }

    public new void DestroyObject(Object objOverride = null)
    {
        Destroy(objOverride != null ? objOverride : gameObject, destroyObjectDelay.GetDelay(true));
    }

    public void DestroyObjectNoDelay(Object objOverride = null)
    {
        Destroy(objOverride != null ? objOverride : gameObject);
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
                blockDirection, transform.position, data.DetectedBy.transform.position,
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
            Debug.Log($"{this} blocked {damage} damage");
        }

        onBlockDamage?.Invoke(damage, data);
    }

/*     protected virtual void MissDamage(int damage, DetectionData data)
    {
        if (debug)
        {
            Debug.Log($"{this} missed {damage} damage");
        }

        missDamageFlag = true;
        onMissDamage?.Invoke(damage, data);
    }
 */

    public virtual void ClearUpdate()
    {
        damageEvents.Clear();
        healEvents.Clear();
        
        dieFlag = false;
        healthChangeFlag = false;
        explosionDamageFlag = false;
    }

    private void OnDisable()
    {
        ClearUpdate();
        StopAllCoroutines();
    }
}

public class DamageEvent
{
    public int amount;
    public DetectionData detectionData;

    public DamageEvent(int amount, DetectionData detectionData)
    {
        this.amount = amount;
        this.detectionData = detectionData;
    }
}

public class HealEvent : DamageEvent
{
    public HealEvent(int amount, DetectionData detectionData) : base(amount, detectionData) { }
}