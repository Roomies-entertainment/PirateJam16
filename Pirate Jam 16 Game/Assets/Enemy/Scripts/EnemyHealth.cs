using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : Health
{
    [Header("")]
    [SerializeField] private UnityEvent<float, DetectionData<Health, Attack>> onStart;

    protected override void Start()
    {
        base.Start();

        onStart?.Invoke(health / maxHealth, null);
    }
}
