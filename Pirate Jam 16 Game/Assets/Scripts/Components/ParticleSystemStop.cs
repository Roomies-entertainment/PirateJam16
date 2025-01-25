using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ParticleSystemStop : MonoBehaviour
{
    public float stopDelay = 1f;
    public ParticleSystem system;

    void Start()
    {
        Invoke("Stop", stopDelay);
    }

    private void Stop()
    {
        if (system == null)
        {
            Debug.Log($"{this} system is null");

            return;
        }

        system.Stop();
    }
}
