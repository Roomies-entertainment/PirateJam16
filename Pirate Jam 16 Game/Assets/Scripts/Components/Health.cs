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

    protected void Awake()
    {
        health = startingHealth;
    }

    protected virtual void IncrementHealth(int increment)
    {
        health = (int) Mathf.Max(0f, health + increment);
    }

    public abstract void ApplyDamage(int damage, ComponentData data);

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
}
