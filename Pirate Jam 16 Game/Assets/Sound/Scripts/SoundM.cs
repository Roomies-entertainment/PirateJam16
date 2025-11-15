using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public static class SoundM
{
    public static ManagerReferences References;

    public static void PlaySound(
        AudioClip clip, Vector2 position,
        float volume = 1.0f, bool loop = false, float pitch = 1.0f, float spatialBlend = 1.0f) {

        if (References == null)
        {
            Debug.Log("SoundM References is null");

            return;
        }

        if (clip == null)
        {
            Debug.Log("Clip is null");
            
            return;
        }

        AudioSource source = new GameObject($"{clip.name} Sound Source").AddComponent<AudioSource>();
        source.transform.position = new Vector3(position.x, position.y, 0.0f);

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
        destroyer.QueueDestruction();
    }

    public static void PlaySound(
        AudioClip clip, float volume = 1.0f, float pitch = 1.0f, bool loop = false) {

        if (References == null)
        {
            Debug.Log("SoundM References is null");

            return;
        }

        if (clip == null)
        {
            Debug.Log("Clip is null");
            
            return;
        }

        AudioSource source = References.nonPositionalSource.AddComponent<AudioSource>();

        source.playOnAwake = true;
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.spatialBlend = 0.0f;
        source.loop = loop;
        source.outputAudioMixerGroup = References.sfxMixerGroup;
        source.Play();

        AudioSourceDestroyer destroyer = source.gameObject.AddComponent<AudioSourceDestroyer>();

        destroyer.source = source;
        destroyer.QueueDestruction();
    }
}
