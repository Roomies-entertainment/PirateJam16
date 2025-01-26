using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{

    public GameObject pauseMenu;

    public bool shouldShow = false;

    void Update()
    {

        if (shouldShow == false)
            return;

        if (Input.GetButtonDown("Cancel") && pauseMenu.activeInHierarchy == false)
        {
            PauseTheGame();
        }
        else if (Input.GetButtonDown("Cancel") && pauseMenu.activeInHierarchy == true)
        {
            ResumeTheGame();
        }
    }

    void PauseTheGame()
    {
        
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    void ResumeTheGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void StartTheGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("01 Level 1");
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("00 Menu");
    }

    public void QuitTheGame()
    {
        Application.Quit();
    }
}
