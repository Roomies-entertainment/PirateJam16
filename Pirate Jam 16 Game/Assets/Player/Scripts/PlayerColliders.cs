using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliders : MonoBehaviour
{
    [SerializeField] private Transform Colliders;

    [Header("")]
    [SerializeField] private Transform UprightConfiguration;
    [SerializeField] private Transform FlatConfiguration;

    public void OnPerformAttack()
    {
        Colliders.position = FlatConfiguration.position;
        Colliders.rotation = FlatConfiguration.rotation;
    }

    public void OnStopAttack()
    {
        Colliders.position = UprightConfiguration.position;
        Colliders.rotation = UprightConfiguration.rotation;
    }
}
