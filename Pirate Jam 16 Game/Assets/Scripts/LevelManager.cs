using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    GameObject difficultyLoad;

    void Start(){
        difficultyLoad = GameObject.FindGameObjectWithTag("difficulty");
        difficultyLoad.GetComponent<DifficultySetting>().SetTheDifficulty();
    }

    public void LoadNextLevel(){
        int nextLevel = SceneManager.loadedSceneCount;
        SceneManager.LoadScene(nextLevel + 1);


    }
}
