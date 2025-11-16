using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioPlaylist : MonoBehaviour
{
    [SerializeField] private AudioSource audioPlayer;
    [SerializeField] private AudioClip[] audioClips;
    private AudioClip currentClip;

    private float audioInterval;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        List<GameObject> objs = GameObjectM.FindGameObjectsWithTag(Tags.TagType.music);

        if (objs.Count > 1)
        {
            Destroy(this.gameObject);

        }

        audioPlayer.clip = audioClips[0];
        currentClip = audioPlayer.clip;
        audioPlayer.Play();
        audioInterval = currentClip.length;

        StartCoroutine(AudioPlaylistCycle());
    }

    void Update()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0))
        {
            Destroy(this.gameObject);
        }
    }


    private void Start()
    {

    }

    IEnumerator AudioPlaylistCycle()
    {

        for (int i = 0; i < audioClips.Length + 1; i = i)
        {
            Debug.Log($"Audio Interval: {audioInterval}");
            yield return new WaitForSeconds(audioInterval);
            i = (i + 1) % audioClips.Length;
            currentClip = audioClips[i];
            audioPlayer.clip = currentClip;
            audioPlayer.Play();
            audioInterval = currentClip.length;
        }
    }
    
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
