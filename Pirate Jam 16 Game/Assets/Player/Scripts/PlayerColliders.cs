using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliders : MonoBehaviour
{
    [SerializeField] private Transform Colliders;

    [Header("")]
    [SerializeField] private Transform UprightConfiguration;
    [SerializeField] private Transform FlatConfiguration;

    private void Start()
    {
        UpdateConfiguration(UprightConfiguration);
    }

    public void OnPerformAttack()
    {
        UpdateConfiguration(FlatConfiguration);
    }

    public void OnStopAttack()
    {
        UpdateConfiguration(UprightConfiguration);
    }

    private void UpdateConfiguration(Transform configuration)
    {
        Colliders.position = configuration.position;
        Colliders.rotation = configuration.rotation;
    }
}
