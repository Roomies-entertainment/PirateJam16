using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : Attack
{
    [SerializeField] protected bool directionChecking;

    [Header("")]
    private Vector2 attackDirection = Vector3.right;

    public void StartAttack()
    {
        List<DetectedComponent<Health>> players;
        var attackDirection = GetAttackDirection(out players);

        if (debug && (players == null || players.Count == 0))
        {
            Debug.Log($"{gameObject.name} in StartAttack() - detectedHealthComponents null or count = 0");

            return;
        }

        transform.position += new Vector3(attackDirection.x, attackDirection.y, 0f) * 0.25f;

        base.StartAttack(players, attackDirection);
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

    private Vector2 GetAttackDirection(out List<DetectedComponent<Health>> players)
    {
        players = Detection.DetectComponent<Health>(
            AttackCircle.transform.position, AttackCircle.GetRadius(), 1 << Collisions.playerLayer);

        if (players.Count == 0)
        {
            return new Vector2();
        }

        Vector2 firstPlayerDir = players[0].Component.transform.position - transform.position;
        firstPlayerDir.y = 0f;
        firstPlayerDir.Normalize();

        return firstPlayerDir;
    }
}
