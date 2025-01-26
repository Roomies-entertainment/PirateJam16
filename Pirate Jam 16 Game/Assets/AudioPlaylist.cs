using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class AudioPlaylist : MonoBehaviour
{
    [SerializeField] private AudioSource audioPlayer;
    [SerializeField] private AudioClip[] audioClips;
    private AudioClip currentClip;

    private float audioInterval;

    private void Start(){
        audioPlayer.clip = audioClips[0];
        currentClip = audioPlayer.clip;
        audioPlayer.Play();
        audioInterval = currentClip.length;
        
        StartCoroutine(AudioPlaylistCycle());
    }

    IEnumerator AudioPlaylistCycle(){
        
        for (int i = 0; i < audioClips.Length + 1; i = i)
        {
            Debug.Log($"Audio Time: {audioInterval}");
            yield return new WaitForSeconds(audioInterval);
            i = (i + 1) % audioClips.Length;
            currentClip = audioClips[i];
            audioPlayer.clip = currentClip;
            audioPlayer.Play();
            audioInterval = currentClip.length;
        }
    }
}
