using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private int BaseDamage = 1;

    public Vector2 GetAttackDirection(out List<Player> players)
    {
        players = Detection.DetectComponent<Player>(transform.position, attackRadius, 1 << Collisions.playerLayer);

        if (players.Count == 0)
            return new Vector2();

        Vector2 firstPlayerDir = (players[0].transform.position - transform.position).normalized;

        return firstPlayerDir;
    }

    public void AttackPlayers(List<Player> players, Vector2 attackDirection)
    {
        foreach (var player in players)
        {
            if ( Vector2.Dot((player.transform.position - transform.position).normalized, attackDirection.normalized) > 0f )
            {
                player.TakeDamage(BaseDamage);
            }
        }
    }
}
