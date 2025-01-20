using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private int BaseDamage = 1;

    public void AttackEnemies(Vector3 attackDir)
    {
        var enemies = PlayerDetection.DetectEnemies(transform.position, attackRadius);

        foreach (var enemy in enemies)
        {
            if ( Vector2.Dot((enemy.transform.position - transform.position).normalized, attackDir.normalized) > 0f )
            {
                Destroy(enemy);
            }
        }
    }
}
