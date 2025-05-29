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
            var obj = Instantiate(prefab, transform.position, transform.rotation);
            var p = GetComponentInChildren<ParticleSystem>();

            if (p != null)
                if (play)
                    p.Play();
                else
                    Destroy(obj);
        }
    }
}
