using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [SerializeField] protected int startingHealth = 3;
    [SerializeField] protected int healthMax = 3;

    public int health { get; protected set; }

    protected void Awake()
    {
        health = Mathf.Max(startingHealth, healthMax);
    }

    protected virtual void IncrementHealth(int increment)
    {
        health = (int) Mathf.Clamp(health + increment, 0f, healthMax);
    }

    public abstract void TakeDamage(int damage);
}
