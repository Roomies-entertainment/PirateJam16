using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    [SerializeField] protected GameObject blockParticlePrefab;

    private void Start() { } // Ensures component toggle in inspector

    public void OnBlockDamage(float damage, DetectionData data)
    {
        ParticleM.SpawnParticle(blockParticlePrefab, data.Point);
    }
}
