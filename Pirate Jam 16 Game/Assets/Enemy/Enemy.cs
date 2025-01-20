using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    private Health Health;

    private void Awake()
    {
        Health = GetComponent<Health>();
    }

    public void TakeDamage(int damage)
    {
        Health.IncrementHealth(-damage);

        Debug.Log($"{this} health has reached {Health.health}");

        if (Health.health == 0)
            Destroy(gameObject);
    }
}
