using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackDelay = 2f;
    [SerializeField] private float attackRadius = 2f;
    [SerializeField] private int BaseDamage = 1;

    private Vector3 attackDirection = Vector3.right; 

    private void Start()
    {
        StartCoroutine(AttackLoop());
    }

    private IEnumerator AttackLoop()
    {
        while (gameObject != null)
        {
            attackDirection = GetAttackDirection(out List<PlayerHealth> players);
            
            transform.position += attackDirection * 0.25f;

            AttackPlayers(players, attackDirection);

            yield return new WaitForSeconds(0.2f);

            transform.position -= attackDirection * 0.25f;

            yield return new WaitForSeconds(attackDelay);
        }
    }

    public Vector2 GetAttackDirection(out List<PlayerHealth> players)
    {
        players = Detection.DetectComponent<PlayerHealth>(transform.position, attackRadius, 1 << Collisions.playerLayer);

        if (players.Count == 0)
            return new Vector2();

        Vector2 firstPlayerDir = players[0].transform.position - transform.position;
        firstPlayerDir.y = 0f;
        firstPlayerDir.Normalize();

        return firstPlayerDir;
    }

    public void AttackPlayers(List<PlayerHealth> players, Vector2 attackDirection)
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
