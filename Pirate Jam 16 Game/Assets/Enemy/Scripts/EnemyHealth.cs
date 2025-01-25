using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    [SerializeField] private float blockDuration = 1.5f;
    [SerializeField] private float blockInterval = 3.0f;

    private void Start()
    {
        StartCoroutine(BlockLoop(blockInterval));
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

    protected override void IncrementHealth(int increment)
    {
        base.IncrementHealth(increment);

        Debug.Log($"{gameObject.name} health has reached {health}");
    }

    public override void ApplyDamage(int damage, ComponentData data)
    {
        TakeDamage(damage, data);
    }

    protected override void TakeDamage(int damage, ComponentData data)
    {
        if (blocking)
        {
            Debug.Log($"{gameObject.name} blocked damage");

            return;
        }

        base.TakeDamage(damage, data);

        if (health == 0)
            Destroy(gameObject);
    }

    public override void StartBlocking()
    {
        base.StartBlocking();
    }

    public override void StopBlocking()
    {
        base.StopBlocking();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
