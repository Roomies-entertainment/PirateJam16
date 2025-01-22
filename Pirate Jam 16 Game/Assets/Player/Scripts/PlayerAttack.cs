using System.Collections.Generic;

using UnityEngine;

public class PlayerAttack : Attack
{
    public List<Health> FindObjectsToAttack(Vector2 attackDirection)
    {
        var enemies = Detection.DetectComponent<EnemyHealth>(transform.position, attackRadius,  1 << Collisions.enemyLayer);
        var objectsR = new List<Health>();

        foreach (var enemy in enemies)
        {
            if ( Vector2.Dot((enemy.transform.position - transform.position).normalized, attackDirection.normalized) > 0f )
            {
                objectsR.Add(enemy);
            }
        }

        return objectsR;
    }
}
