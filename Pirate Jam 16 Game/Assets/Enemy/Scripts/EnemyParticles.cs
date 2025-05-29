using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParticles : MonoBehaviour
{
    [Header("")]
    [SerializeField] protected GameObject bloodParticlePrefab;
    [SerializeField] protected GameObject blockParticlePrefab;

    private void Start() { } // Ensures component toggle in inspector
    
    public void OnTakeDamage(float damage, DetectionData data)
    {
        ParticleM.SpawnParticle(bloodParticlePrefab, data.Point, data.DetectorComponent.transform, true, 0.15f);
    }

    public void OnBlockDamage(float damage, DetectionData data)
    {
        ParticleM.SpawnParticle(blockParticlePrefab, data.Point);
    }
}
