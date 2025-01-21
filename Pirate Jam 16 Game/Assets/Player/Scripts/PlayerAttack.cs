using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private int BaseDamage = 1;

    [Header("")]
    [SerializeField] private AudioClip attackSound;

    public void AttackEnemies(Vector3 attackDirection)
    {
        var enemies = Detection.DetectComponent<EnemyHealth>(transform.position, attackRadius,  1 << Collisions.enemyLayer);

        foreach (var enemy in enemies)
        {
            if ( Vector2.Dot((enemy.transform.position - transform.position).normalized, attackDirection.normalized) > 0f )
            {
                enemy.TakeDamage(BaseDamage);
            }
        }

        SoundManager.PlaySoundNonSpatial(attackSound, 0.6f, 0.8f);
    }
}
