using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RandomizedSound : MonoBehaviour
{
    int index;
    [SerializeField] private AudioClip[] audioClips;

    public void PlaySound(){
        SoundManager.PlaySoundNonSpatial(audioClips[index]);
    }
}
