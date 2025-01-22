using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : Attack
{
    [SerializeField] private float attackDelay = 2f;

    private Vector3 attackDirection = Vector3.right;

    private void Start()
    {
        StartCoroutine(AttackLoop());
    }

    private IEnumerator AttackLoop()
    {
        while (gameObject != null)
        {
            attackDirection = GetAttackDirection(out List<ComponentData> players);
            
            transform.position += attackDirection * 0.25f;

            PerformAttack(players, attackDirection);

            yield return new WaitForSeconds(0.2f);

            transform.position -= attackDirection * 0.25f;

            StopAttack();

            yield return new WaitForSeconds(attackDelay);
        }
    }

    public Vector2 GetAttackDirection(out List<ComponentData> players)
    {
        players = Detection.DetectComponent<Health>(transform.position, attackRadius, 1 << Collisions.playerLayer);

        if (players.Count == 0)
            return new Vector2();

        Vector2 firstPlayerDir = players[0].component.transform.position - transform.position;
        firstPlayerDir.y = 0f;
        firstPlayerDir.Normalize();

        return firstPlayerDir;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
