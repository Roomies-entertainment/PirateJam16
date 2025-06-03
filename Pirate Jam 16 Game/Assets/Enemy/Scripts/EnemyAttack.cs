using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAttack : Attack
{
    [Header("")]
    [SerializeField] private bool damagePlayers = true;
    [SerializeField] private bool damageEnemies = false;

    private void Start() { } // Ensures component toggle in inspector
    
    protected override List<System.Type> GetDetectableTypes()
    {
        List<System.Type> types = base.GetDetectableTypes();

        if (damagePlayers)
            types.Add(typeof(PlayerHealth));

        if (damageEnemies)
            types.Add(typeof(EnemyHealth));

        return types;
    }

    public override void StopAttack()
    {
        base.StopAttack();

        if (!attacking)
        {
            return;
        }

        transform.position -= new Vector3(attackDirection.x, attackDirection.y, 0f) * 0.25f;
    }

    public Vector2 GetAttackDirection()
    {
        Vector2 toFirst = foundComps.Keys.First().transform.position - transform.position;

        toFirst.y = 0f;
        toFirst.Normalize();

        return toFirst;
    }
}
