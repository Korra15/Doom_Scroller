using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Security.Cryptography;


public class GameOver_Manager : MonoBehaviour
{
    public GameObject GameOverUI;
    public TMP_Text ScoreText;

    private void Start()
    {
        setScoreText((int)PersistentGameData.score);
    }
    public void StartGame_Btn()
    {
        PersistentGameData.score = 0f;
        SceneManager.LoadScene(2);
        
    }

    public void setScoreText(int score)
    {
        ScoreText.text = score.ToString();
    }

    public void QuitGame()
    {
        // Quit the game (this only works in a built game, not in the editor)
        Application.Quit();
    }
}


