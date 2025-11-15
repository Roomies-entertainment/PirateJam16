using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttack : Attack
{
    [Tooltip("Don't hit objects beneath")]
    [SerializeField] protected bool beneathDirCheck = false;
    [Range(-1, 1)]
    [SerializeField] protected float beneathDirCheckLeniance = 0.5f;

    [Header("")]
    [SerializeField] private SpawnProjectile ProjectileSpawnComponent;

    protected override bool CanHitObject(GameObject obj)
    {
        if (!AttackDirectionHit(obj.transform.position))
            return false;

        return true;
    }

    protected override bool AttackDirectionHit(Vector2 objPosition)
    {
        return (!beneathDirCheck || Detection.DirectionCheck(Vector2.up, transform.position, objPosition, true, beneathDirCheckLeniance)) &&
                base.AttackDirectionHit(objPosition);
    }
    protected override void AttackAndInteract(int damage = BaseDamage)
    {
        Vector2 pos = ProjectileSpawnComponent.spawnPoint.position + new Vector3(attackDirection.x, 0f, 0f);
        ProjectileSpawnComponent.Spawn(pos.x, pos.y, attackDirection);
    }
}
