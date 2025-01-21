using UnityEngine;

public class PlayerAttack : Attack
{
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

        SoundManager.PlaySoundNonSpatial(attackSound);

        onAttack.Invoke();
    }
}
