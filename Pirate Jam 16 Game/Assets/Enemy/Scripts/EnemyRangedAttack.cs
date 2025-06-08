using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttack : Attack
{

    [SerializeField] private SpawnProjectile ProjectileSpawn;

    protected override bool CanHitObject(GameObject obj)
    {
        if (!AttackDirectionHit(obj.transform.position))
            return false;

        return true;
    }

    protected override void AttackAndInteract(int damage = BaseDamage)
    {
        Vector2 pos = ProjectileSpawn.spawnPoint.position + new Vector3(attackDirection.x, 0f, 0f);
        ProjectileSpawn.Spawn(pos.x, pos.y, attackDirection);
    }
}
