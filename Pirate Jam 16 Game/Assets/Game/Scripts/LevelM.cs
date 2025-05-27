using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelM : MonoBehaviour
{
    private float audioInterval;

    [SerializeField] private AudioSource audioPlayer;
    [SerializeField] private AudioClip audioClip;

    GameObject difficultyLoad;

    void Start(){

        audioPlayer.clip = audioClip;

        difficultyLoad = GameObjectM.FindGameObjectWithTag(Tags.TagType.Difficulty);
        difficultyLoad.GetComponent<DifficultySetting>().SetTheDifficulty();
    }

    public void LoadNextLevel(){
        audioInterval = audioClip.length;
        StartCoroutine(StartNextLevel());

    }

    IEnumerator StartNextLevel(){
        audioPlayer.enabled = true;
        yield return new WaitForSeconds(audioInterval + 0.2f);

        int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextLevel);
    }
}
