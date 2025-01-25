using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    [SerializeField] protected GameObject blockParticlePrefab;

    public void OnBlockDamage(float damage, DetectionData data)
    {
        ParticleManager.SpawnParticle(blockParticlePrefab, data.Point);
    }
}
