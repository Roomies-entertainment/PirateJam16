using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    [SerializeField] private float blockDuration = 1.5f;
    [SerializeField] private float blockInterval = 3.0f;


    public HealthBar healthBar;

    private void Start()
    {
        StartCoroutine(BlockLoop(blockInterval));
        healthBar.barCurrentHealth = health;
        healthBar.SetMaxHealth();
    }

    private IEnumerator BlockLoop(float interval)
    {
        while (gameObject != null)
        {
            yield return new WaitForSeconds(interval);

            StartBlocking();

            yield return new WaitForSeconds(blockDuration);

            StopBlocking();
        }
    }

    protected override void TakeDamage(int damage, DetectionData data)
    {
        base.TakeDamage(damage, data);

        healthBar.barCurrentHealth = health;
        healthBar.SetHealth();

        if (health == 0)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
