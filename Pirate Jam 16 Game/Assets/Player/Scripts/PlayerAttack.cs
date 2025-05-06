using System.Collections.Generic;

using UnityEngine;

public class PlayerAttack : Attack
{
    [Range(0, 1)] public float fallingThreshold = 0.24f;
    public int fallingExtraDamage = 1;

    [Header("")]
    public float AttackDuration = 0f;
    public float attackTimer {get; private set; } = 0.0f;

    public List<Health> attackedEnemies { get; private set; } = new List<Health>();

    private void Update()
    {
        attackTimer += Time.deltaTime;
    }

    public List<DetectedComponent> DetectEnemyHealthComponents(Vector2 attackDirection)
    {
        var enemyHCs = Detection.DetectComponent<EnemyHealth>(transform.position, attackRadius, 1 << Collisions.enemyLayer);
        var objectsR = new List<DetectedComponent>();

        foreach (var enemy in enemyHCs)
        {
            var enemyHealth = (Health) enemy.Component;

            if (attackedEnemies.Contains(enemyHealth))
                continue;
                
            if ( Vector2.Dot((enemyHealth.transform.position - transform.position).normalized, attackDirection.normalized) > 0f )
            {
                objectsR.Add(new DetectedComponent(enemyHealth, enemy.Colliders));
            }
        }

        return objectsR;
    }

    protected override void OnPerformAttack(Vector2 direction)
    {
        base.OnPerformAttack(direction);

        attackTimer = 0.0f;
    }

    protected override void OnAttackObject(GameObject enemy)
    {
        attackedEnemies.Add(enemy.GetComponent<Health>());
    }

    protected override void OnStopAttack()
    {
        base.OnStopAttack();

        attackedEnemies.Clear();
    }
}
