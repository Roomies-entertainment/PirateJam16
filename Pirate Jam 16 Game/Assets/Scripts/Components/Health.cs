using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Health : MonoBehaviour
{
    [SerializeField] protected int startingHealth = 3;
    [SerializeField] protected int healthMax = 3;
    public int health { get; protected set; }

    [Header("")]
    [SerializeField] protected AudioClip attackSound;

    [Header("")]
    [SerializeField] private UnityEvent onTakeDamage;

    protected void Awake()
    {
        health = Mathf.Max(startingHealth, healthMax);
    }

    protected virtual void IncrementHealth(int increment)
    {
        health = (int) Mathf.Clamp(health + increment, 0f, healthMax);
    }

    public virtual void TakeDamage(int damage)
    {
        IncrementHealth(-damage);
        OnTakeDamage();
    }

    public virtual void OnTakeDamage()
    {
        onTakeDamage.Invoke();
    }
}
