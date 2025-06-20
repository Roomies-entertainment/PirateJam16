using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    [SerializeField] protected GameObject blockParticlePrefab;
    [SerializeField] protected ParticleSystem effectParticles;

    private void Start() { } // Ensures component toggle in inspector


    public void OnBlockDamage(float damage, DetectionData data)
    {
        ParticleM.SpawnParticle(blockParticlePrefab, data.Point);
    }

    public void MoveEffectParticles(Vector2 offset, Vector3 rot)
    {
        effectParticles.transform.localPosition = offset;
        effectParticles.transform.localRotation = Quaternion.Euler(rot);
    }
}
