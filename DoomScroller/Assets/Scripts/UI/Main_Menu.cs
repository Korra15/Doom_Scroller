using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Security.Cryptography;


public class Main_Menu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public bool isPaused = false;

    public void StartGame_Onboarding()
    {
        PersistentGameData.score = 0f;
        SceneManager.LoadScene(1);

    }
    public void StartGame_Btn()
    {
        PersistentGameData.score = 0f;
        SceneManager.LoadScene(2);
       
    }

    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;  
        isPaused = false;
    }

    void Pause()
    {

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;  
        isPaused = true;
    }

    public void QuitGame()
    {
        // Quit the game (this only works in a built game, not in the editor)
        Application.Quit();
    }
}


