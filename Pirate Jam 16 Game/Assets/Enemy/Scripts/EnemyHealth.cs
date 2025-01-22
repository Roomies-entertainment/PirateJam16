using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    protected override void IncrementHealth(int increment)
    {
        base.IncrementHealth(increment);

        Debug.Log($"{gameObject.name} health has reached {health}");
    }

    public override void ApplyDamage(int damage, ComponentData data)
    {
        TakeDamage(damage, data);
    }

    protected override void TakeDamage(int damage, ComponentData data)
    {
        base.TakeDamage(damage, data);

        if (health == 0)
            Destroy(gameObject);
    }
}
