using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ParticleManager
{
    public static void SpawnParticle(GameObject particlePrefab, Vector2 position, Transform parent = null, bool startPlaying = true)
    {
        ParticleSystem system = GameObject.Instantiate(particlePrefab, position, default, parent).GetComponent<ParticleSystem>();

        if (system == null)
            return;

        if (startPlaying)
            system.Play();
    }
}
