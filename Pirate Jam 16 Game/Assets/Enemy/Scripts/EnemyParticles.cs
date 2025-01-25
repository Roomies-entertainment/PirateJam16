using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParticles : MonoBehaviour
{
    [Header("")]
    [SerializeField] protected GameObject bloodParticlePrefab;
    [SerializeField] protected GameObject blockParticlePrefab;

    public void OnTakeDamage(float damage, DetectionData data)
    {
        ParticleManager.SpawnParticle(bloodParticlePrefab, data.Point, data.DetectorComponentData.Component.transform, true, 0.15f);
    }

    public void OnBlockDamage(float damage, DetectionData data)
    {
        ParticleManager.SpawnParticle(blockParticlePrefab, data.Point);
    }
}
