using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemSpawn : MonoBehaviour
{
    [SerializeField] private GameObject particlePrefab;

    public void SpawnAndPlayParticles()
    {
        SpawnParticles().Play();
    }

    public ParticleSystem SpawnParticles()
    {
        var particleSystem = Instantiate(particlePrefab, transform.position, transform.rotation).GetComponentInChildren<ParticleSystem>();
        
        if (particleSystem == null)
            return null;

        return particleSystem;
    }
}
