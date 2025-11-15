using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyMeleeAttack : MeleeAttack
{
    public override void StopAttack()
    {
        base.StopAttack();

        if (!attackFlag)
        {
            return;
        }

        transform.position -= new Vector3(attackDirection.x, attackDirection.y, 0f) * 0.25f;
    }
}
