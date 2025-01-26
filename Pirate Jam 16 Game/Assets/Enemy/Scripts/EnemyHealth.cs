using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : Health
{
    [SerializeField] private UnityEvent<float, DetectionData> onStart;

    [SerializeField] private float blockDuration = 1.5f;
    [SerializeField] private float blockInterval = 3.0f;

    private void Start()
    {
        StartCoroutine(BlockLoop(blockInterval));

        onStart.Invoke(health / startingHealth, null);
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

        if (health == 0)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
