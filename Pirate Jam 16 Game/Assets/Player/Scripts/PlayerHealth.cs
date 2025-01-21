using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    public bool blocking;

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
        
        IncrementHealth(-damage);
    }
}
