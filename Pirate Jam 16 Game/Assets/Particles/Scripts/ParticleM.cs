using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ParticleM
{
    public static void SpawnParticle(GameObject particlePrefab, Vector2 position, Transform parent = null,
    bool startPlaying = true, float stopAfter = 0f, float zSortingOffset = 0.01f)
    {
        if (parent == null)
            parent = ManagerReferences.references.particleParent;

        ParticleSystem system = GameObject.Instantiate(particlePrefab, new Vector3(position.x, position.y, -zSortingOffset), default, parent).GetComponent<ParticleSystem>();

        if (system == null)
            return;

        if (startPlaying)
            system.Play();
        
        if (stopAfter > 0)
        {
            var stopScript = system.gameObject.AddComponent<ParticleSystemStop>();

            stopScript.system = system;
            stopScript.stopDelay = stopAfter;
        }   
    }
}
