using System.Collections.Generic;

using UnityEngine;

public class PlayerAttack : Attack
{
    [Range(0, 1)] public float fallingThreshold = 0.24f;
    public int fallingExtraDamage = 1;

    [Header("")]
    public float AttackCooldown = 0f;
    public float attackCooldownTimer {get; private set; } = 0.0f;

    public List<Health> attackedEnemies { get; private set; } = new List<Health>();

    private void Update()
    {
        attackCooldownTimer += Time.deltaTime;
    }

    public List<ComponentData> FindObjectsToAttack(Vector2 attackDirection)
    {
        var enemies = Detection.DetectComponent<EnemyHealth>(transform.position, attackRadius, 1 << Collisions.enemyLayer);
        var objectsR = new List<ComponentData>();

        foreach (var enemy in enemies)
        {
            var enemyHealth = (Health) enemy.component;

            if (attackedEnemies.Contains(enemyHealth))
                continue;
                
            if ( Vector2.Dot((enemyHealth.transform.position - transform.position).normalized, attackDirection.normalized) > 0f )
            {
                objectsR.Add(new ComponentData(enemyHealth, enemy.colliders));
            }
        }

        return objectsR;
    }

    protected override void OnPerformAttack()
    {
        base.OnPerformAttack();

        attackCooldownTimer = 0.0f;
    }

    protected override void OnAttack(Health enemy)
    {
        attackedEnemies.Add(enemy);
    }

    protected override void OnStopAttack()
    {
        base.OnStopAttack();

        attackedEnemies.Clear();
    }
}
