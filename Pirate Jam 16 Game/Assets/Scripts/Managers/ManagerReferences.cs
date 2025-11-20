using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ManagerReferences : MonoBehaviour
{
    public static ManagerReferences references;

    [Header("SoundManager")]
    public AudioMixerGroup sfxMixerGroup;
    public AudioMixerGroup musicMixerGroup;

    public GameObject nonPositionalSource { get; private set; }
    public Transform particleParent { get; private set; } 

    private void Awake()
    {
        references = this;

        nonPositionalSource = new GameObject("Non Positional Sound Sources");
        particleParent      = new GameObject("Spawned Particles").transform;
    }
}
