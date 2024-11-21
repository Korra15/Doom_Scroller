using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class UIManager : MonoBehaviour
{
    public float score;

    [SerializeField] private GameObject _instructionUI;
    [SerializeField] private GameObject _startGameUI;
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private TextMeshProUGUI _scoreText;

    private bool isInstructionUI;

    private void Update()
    {
        if (InputManager.instance.InstructionUIOpenCloseInput)
        {
            if(!isInstructionUI) OpenInstructionUI();
            else CloseInstructionUI();
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        print("Quit Game!");
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenGameOverUI()
    {
        SetScoreText(score);
        _gameOverUI.SetActive(true);    
    }

    private void CloseAllMenus()
    {
        _startGameUI.SetActive(false);
        _gameOverUI.SetActive(false);   
        _instructionUI.SetActive(false);
    }
    private void OpenInstructionUI()
    {
        isInstructionUI = true;
        Time.timeScale = 0f;
        _instructionUI.SetActive(true);
    }

    private void CloseInstructionUI()
    {
        isInstructionUI = false;
        _instructionUI.SetActive(false);
        Time.timeScale = 1f;
    }

    private void SetScoreText(float score)
    {
        _scoreText.text = score.ToString();
    }

}
