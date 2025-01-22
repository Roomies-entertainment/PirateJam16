using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    [HideInInspector] public bool blocking;

    protected override void IncrementHealth(int increment)
    {
        base.IncrementHealth(increment);

        Debug.Log($"{gameObject.name} health has reached {health}");
    }

    public override void TakeDamage(int damage)
    {
        if (blocking)
        {
            Debug.Log($"{gameObject.name} blocked damage");

            return;
        }
        
        base.TakeDamage(damage);
    }
}
