using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultySetting : MonoBehaviour
{

    int difficulty;

    GameObject player;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        GameObject[] objs = GameObject.FindGameObjectsWithTag("difficulty");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);

        }
    }


    public void SetTheDifficulty()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        SetDifficulty();
    }

    public void EasySetting(){

        difficulty = 1;
        
    }

    public void MediumSetting()
    {
        difficulty = 2;
        
    }

    public void HardSetting(){
        difficulty = 3;
        

    }

    void SetDifficulty(){
        switch (difficulty){
            case 3:
                if (player == null)
                    return;
                player.GetComponent<PlayerHealth>().DifficultySet(1);
                Debug.Log("Difficulty Hard Set");
                //hard Mode
                break;

            case 2:
                if (player == null)
                    return;
                player.GetComponent<PlayerHealth>().DifficultySet(6);
                Debug.Log("Difficulty Normal Set");
                //Medium Mode
                break;

            case 1:
                if (player == null)
                    return;
                player.GetComponent<PlayerHealth>().DifficultySet(10);
                Debug.Log("Difficulty Easy Set");
                // Easy Mode
                break;

            default:
                print("Invalid selection");

                break;
        }
    }
}
