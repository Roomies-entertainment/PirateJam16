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

    protected override void OnDie()
    {
        base.OnDie();

        Destroy(gameObject);
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

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
