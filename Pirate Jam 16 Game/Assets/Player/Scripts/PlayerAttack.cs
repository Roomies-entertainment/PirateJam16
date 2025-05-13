using System.Collections.Generic;

using UnityEngine;

public class PlayerAttack : Attack
{
    [Header("")]
    [Range(0, 1)] public float fallingThreshold = 0.24f;
    public int fallingExtraDamage = 1;

    [Header("")]
    public float AttackDuration = 0f;
    public float attackTimer {get; private set; } = 0.0f;

    private Vector2 attackDirection;
    public List<Health> attackedEnemies { get; private set; } = new List<Health>();

    private void Update()
    {
        attackTimer += Time.deltaTime;
    }

    public List<DetectedComponent<Health>> FindHealthComponents(Vector2 attackDirection)
    {
        var enemyHCs = Detection.DetectComponent<EnemyHealth>(
            AttackCircle.transform.position, AttackCircle.GetRadius(), 1 << Collisions.enemyLayer);

        var objectsR = new List<DetectedComponent<Health>>();

        foreach (var enemy in enemyHCs)
        {
            var enemyHealth = (Health) enemy.Component;

            if (attackedEnemies.Contains(enemyHealth))
                continue;
                
            if ( attackDirection.sqrMagnitude == 0 || Vector2.Dot((enemyHealth.transform.position - transform.position).normalized, attackDirection.normalized) > 0f )
            {
                objectsR.Add(new DetectedComponent<Health>(enemyHealth, enemy.Colliders));
            }
        }

        return objectsR;
    }

    protected override void OnStartAttack(Vector2 direction)
    {
        base.OnStartAttack(direction);

        attackTimer = 0.0f;
    }

    protected override void OnHitObject(GameObject enemy)
    {
        attackedEnemies.Add(enemy.GetComponent<Health>());

        base.OnHitObject(enemy);
    }

    protected override void OnStopAttack()
    {
        base.OnStopAttack();

        attackedEnemies.Clear();
    }
}
