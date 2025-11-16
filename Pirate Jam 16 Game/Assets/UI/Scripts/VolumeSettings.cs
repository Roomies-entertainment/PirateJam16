using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start() {
        
        LoadVolume();
    }

    private void LoadVolume() {

        musicSlider.value = PlayerPrefM.GetFloat("musicVolume", 1.0f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1.0f);

        SetMusicVolume();
        SetSFXVolume();
    }

    public void SetMusicVolume() {

        float volume = musicSlider.value;
        myMixer.SetFloat("music", LinearToMixer(volume));
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume() {

        float volume = sfxSlider.value;
        myMixer.SetFloat("sfx", LinearToMixer(volume));
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public static float LinearToMixer(float volume) {

        return Mathf.Log10(volume) * 20f;
    }
}
