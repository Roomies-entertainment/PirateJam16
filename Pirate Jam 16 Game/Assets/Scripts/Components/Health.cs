using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Health : MonoBehaviour
{
    [SerializeField] protected int startingHealth = 3;
    public int health { get; protected set; }


    [Header("")]
    [SerializeField] protected AudioClip damageSound;
    [SerializeField] protected AudioClip blockSound;
    [SerializeField] protected AudioClip deathSound;

    [Header("")]
    [SerializeField] protected Collider2D[] TakeDamageColliders;
    [SerializeField] protected Collider2D[] BlockDamageColliders;

    [Header("")]
    [SerializeField] private UnityEvent<float, DetectionData> onTakeDamage;
    [SerializeField] private UnityEvent<float, DetectionData> onBlockDamage;
    [SerializeField] private UnityEvent onDie;

    public bool blocking { get; private set; }

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
        if (deathSound != null)
            SoundManager.PlaySoundNonSpatial(deathSound);
            
        onDie.Invoke();
    }

    public bool ApplyDamage(int damage, DetectionData data)
    {
        bool colTakeDamage = false;
        bool colBlockDamage = false;

        foreach(var c in TakeDamageColliders)
            if (data.DetectedComponentData.Colliders.Contains(c))
            {
                colTakeDamage = true;

                break;
            }

        if (!colTakeDamage)
        {
            BlockDamage(damage, data);

            return false;
        }

        foreach(var c in TakeDamageColliders)
            if (data.DetectedComponentData.Colliders.Contains(c))
            {
                colTakeDamage = true;

                break;
            }

        if (!colTakeDamage)
            return false;

        foreach(var c in BlockDamageColliders)
            if (data.DetectedComponentData.Colliders.Contains(c))
            {
                colBlockDamage = true;

                break;
            }

        if (blocking || colBlockDamage)
        {
            BlockDamage(damage, data);

            return false;
        }

        TakeDamage(damage, data);
        
        return true;
    }

    protected virtual void TakeDamage(int damage, DetectionData data)
    {
        Debug.Log($"{gameObject.name} took {damage} damage");

        IncrementHealth(-damage);

        if (damageSound != null)
            SoundManager.PlaySoundNonSpatial(damageSound);

        onTakeDamage.Invoke((float) health / startingHealth, data);
    }

    protected virtual void BlockDamage(int damage, DetectionData data)
    {
        Debug.Log($"{gameObject.name} blocked {damage} damage");

        if (blockSound != null)
            SoundManager.PlaySoundNonSpatial(damageSound);

        onBlockDamage.Invoke(damage, data);
    }

    public virtual void StartBlocking()
    {
        Debug.Log($"{gameObject.name} is blocking");
        blocking = true;
    }

    public virtual void StopBlocking()
    {
        Debug.Log($"{gameObject.name} stopped blocking");
        blocking = false;
    }
}
