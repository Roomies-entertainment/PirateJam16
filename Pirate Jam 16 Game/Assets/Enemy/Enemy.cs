using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(EnemyAttack))]
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    public float attackDelay = 0.8f;

    private EnemyAttack Attack;
    private Health Health;

    private Vector3 attackDirection = Vector3.right; 

    private void Awake()
    {
        Attack = GetComponent<EnemyAttack>();
        Health = GetComponent<Health>();
    }

    private void Start()
    {
        StartCoroutine(AttackLoop());
    }

    private IEnumerator AttackLoop()
    {
        while (!gameObject.IsDestroyed())
        {
            transform.position += attackDirection * 0.25f;

            attackDirection = Attack.GetAttackDirection(out List<Player> players);
            Attack.AttackPlayers(players, attackDirection);

            yield return new WaitForSeconds(0.2f);

            transform.position -= Vector3.right * 0.25f;

            yield return new WaitForSeconds(attackDelay);
        }
    }

    public void TakeDamage(int damage)
    {
        Health.IncrementHealth(-damage);

        Debug.Log($"{this} health has reached {Health.health}");

        if (Health.health == 0)
            Destroy(gameObject);
    }
}
