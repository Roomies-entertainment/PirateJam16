using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealCollider : MonoBehaviour
{
    [SerializeField] private int healthGiven = 1;

    public void Heal(Collider2D collider)
    {
        var health = collider.GetComponentInParent<Health>();

        if (health)
        {
            health.IncrementHealth(healthGiven, new DetectionData(transform.position, health, this));
        }
    }
}
