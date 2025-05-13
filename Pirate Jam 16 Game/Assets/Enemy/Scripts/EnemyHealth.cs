using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : Health
{
    [Header("")]
    [SerializeField] private UnityEvent<float, DetectionData<Health, Attack>> onStart;

    private void Start()
    {
        onStart?.Invoke(health / startingHealth, null);
    }

    protected override void OnDie()
    {
        base.OnDie();

        Destroy(gameObject);
    }
}
