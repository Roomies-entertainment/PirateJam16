using UnityEngine;

public class RandomizedSound : MonoBehaviour
{
    [SerializeField] private float volume = 1f;
    [SerializeField] private float pitchMin = 0.9f;
    [SerializeField] private float pitchMax = 1f;
    [SerializeField] private bool loop = false;

    [Header("")]
    [SerializeField] private AudioClip[] audioClips;

    public void PlaySound()
    {
        if (audioClips.Length == 0)
            return;

        SoundM.PlaySound(audioClips[RandomM.Range(0, audioClips.Length)], volume, RandomM.Range(pitchMin, pitchMax), loop);
    }
}