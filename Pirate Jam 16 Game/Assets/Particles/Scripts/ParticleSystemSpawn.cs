using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemSpawn : MonoBehaviour
{
    [SerializeField] private List<GameObject> particlePrefabs = new();

    public void SpawnParticles()
    {
        SpawnParticles(false);
    }

    public void SpawnAndPlayParticles()
    {
        SpawnParticles(true);
    }

    private void SpawnParticles(bool play)
    {
        foreach (var prefab in particlePrefabs)
        {
            ParticleM.SpawnParticle(prefab, transform.position, null, play);
        }
    }
}
