using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParticles : MonoBehaviour
{
    [Header("")]
    [SerializeField] protected GameObject bloodParticlePrefab;

    public void OnTakeDamage(float damage, DetectionData data)
    {
        ParticleManager.SpawnParticle(bloodParticlePrefab, data.Point, data.DetectorComponentData.Component.transform, true, 0.3f);
    }
}
