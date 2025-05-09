using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ManagerReferences : MonoBehaviour
{
    [Header("SoundManager")]
    public AudioMixerGroup sfxMixerGroup;
    public AudioMixerGroup musicMixerGroup;

    public GameObject nonPositionalSource { get; private set; }

    private void Awake()
    {
        SoundM.References = this;

        nonPositionalSource = new GameObject("Non Positional Sound Sources");
    }
}
