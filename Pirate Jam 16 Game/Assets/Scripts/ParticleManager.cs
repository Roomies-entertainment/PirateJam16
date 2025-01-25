using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ParticleManager
{
    public static void SpawnParticle(GameObject particlePrefab, Vector2 position, Transform parent = null,
    bool startPlaying = true, float stopAfter = 0f)
    {
        ParticleSystem system = GameObject.Instantiate(particlePrefab, position, default, parent).GetComponent<ParticleSystem>();

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
