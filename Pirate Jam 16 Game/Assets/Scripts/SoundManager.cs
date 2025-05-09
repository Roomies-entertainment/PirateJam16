using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public static class SoundManager
{
    public static ManagerReferences References;

    public static void PlaySoundSpatial(AudioClip clip, Vector3 position, float volume = 1.0f, bool loop = false, float pitch = 1.0f, float spatialBlend = 0f)
    {
        if (clip == null)
        {
            Debug.Log("Clip is null");
            
            return;
        }

        AudioSource source = new GameObject($"{clip.name} Sound Source").AddComponent<AudioSource>();

        source.playOnAwake = true;
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.spatialBlend = spatialBlend;
        source.loop = loop;
        source.outputAudioMixerGroup = References.sfxMixerGroup;
        source.Play();

        AudioSourceDestroyer destroyer = source.gameObject.AddComponent<AudioSourceDestroyer>();

        destroyer.source = source;
        destroyer.destroyObjectOnCompletion = true;
        destroyer.QueueDestruction();
    }

    public static void PlaySoundNonSpatial(AudioClip clip, float volume = 1.0f, float pitch = 1.0f, bool loop = false)
    {
        if (clip == null)
        {
            Debug.Log("Clip is null");
            
            return;
        }

        if (References == null)
        {
            return;
        }

        AudioSource source = References.nonPositionalSource.AddComponent<AudioSource>();

        source.playOnAwake = true;
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.spatialBlend = 0f;
        source.loop = loop;
        source.outputAudioMixerGroup = References.sfxMixerGroup;
        source.Play();

        AudioSourceDestroyer destroyer = source.gameObject.AddComponent<AudioSourceDestroyer>();

        destroyer.source = source;
        destroyer.QueueDestruction();
    }
}
