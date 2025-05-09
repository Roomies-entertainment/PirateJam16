using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RandomizedSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;

    public void PlaySound(){
        SoundM.PlaySoundNonSpatial(audioClips[Random.Range(0, audioClips.Length)]);
    }
}
