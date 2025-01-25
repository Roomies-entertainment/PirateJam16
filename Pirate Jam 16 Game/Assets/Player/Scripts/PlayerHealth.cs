using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : Health
{
    public Collider2D HandleCollider;
    public Collider2D BladeCollider;

    private void Start()
    {
        if (HandleCollider == null)
            Debug.LogError($"{gameObject.name} Handle collider is null");

        if (HandleCollider == null)
            Debug.LogError($"{gameObject.name} Blade collider is null");
    }

    protected override void IncrementHealth(int increment)
    {
        base.IncrementHealth(increment);

        Debug.Log($"{gameObject.name} health has reached {health}");
    }

    public override void ApplyDamage(int damage, ComponentData data)
    {
        foreach(var c in data.colliders)
            Debug.Log(c);

        if (blocking || !data.colliders.Contains(HandleCollider))
        {
            Debug.Log($"{gameObject.name} blocked damage");

            BlockDamage(damage, data);

            return;
        }
        
        TakeDamage(damage, data);
    }
}
