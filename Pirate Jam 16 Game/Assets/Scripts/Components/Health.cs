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

    [Header("")]
    [SerializeField] private UnityEvent onTakeDamage;
    [Header("")]
    [SerializeField] private UnityEvent onBlockDamage;

    [SerializeField] protected Collider2D[] BlockDamageColliders;

    public bool blocking { get; private set; }

    protected void Awake()
    {
        health = startingHealth;
    }

    protected virtual void IncrementHealth(int increment)
    {
        health = (int) Mathf.Max(0f, health + increment);
    }

    public bool ApplyDamage(int damage, ComponentData data)
    {
        bool colliderBlocking = false;

        foreach(var c in BlockDamageColliders)
            if (data.colliders.Contains(c))
            {
                colliderBlocking = true;

                break;
            }

        if (blocking || colliderBlocking)
        {
            Debug.Log($"{gameObject.name} blocked damage");

            BlockDamage(damage, data);

            return false;
        }

        TakeDamage(damage, data);
        
        return true;
    }

    protected virtual void TakeDamage(int damage, ComponentData data)
    {
        IncrementHealth(-damage);

        if (damageSound != null)
            SoundManager.PlaySoundNonSpatial(damageSound);

        onTakeDamage.Invoke();
    }

    protected virtual void BlockDamage(int damage, ComponentData data)
    {
        if (blockSound != null)
            SoundManager.PlaySoundNonSpatial(damageSound);

        onBlockDamage.Invoke();
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
