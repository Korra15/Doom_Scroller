using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Windows;
using JetBrains.Annotations;
using UnityEngine.Video;

public class Pause : MonoBehaviour
{
    public bool paused;
    

    [Header("Pause Menu Canvas")]
    public GameObject canvas;

    [Header("Attached UI Elements")]
    public GameObject pausemenu;

    [Header("Attached Systems")]
    public EventSystem eventSystem;
    public UserInput input;
    public SaveManager save;

    [Header("Various Menus")]
    public GameObject SettingMenu;
    public GameObject ControlMenu;
    public GameObject VolumeMenu;

    [Header("Selected Buttons")]
    public GameObject resumebutton;
    public GameObject settingsbutton;
    public GameObject controlsbutton;
    public GameObject VolumeSlider;
    public PlayerStateManager playerState;

    void Start(){
        if(playerState == null){
            playerState = GameObject.Find("Player").GetComponent<PlayerStateManager>();
        }

        if(eventSystem == null){
            eventSystem = GameObject.Find("InputManager").GetComponent<EventSystem>();
        }

        if(save == null){
            save = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        }

        if(input == null){
            input = GameObject.Find("InputManager").GetComponent<UserInput>();
        }
    }

    void Update()
    {
        if(input.MenuOpenClose || input.Pause)
        {
            if (paused)
            {
                Resume();
            }
            else
            {
                Pausing();
            }
        }

    }

    public void Resume()
    {
        paused = false;
        canvas.SetActive(false);
        pausemenu.SetActive(true);
        SettingMenu.SetActive(false);
        ControlMenu.SetActive(false);
        VolumeMenu.SetActive(false);

        Time.timeScale = 1f;    
        input.inGameActionMapEnable();
    }

    public void Pausing()
    {
        if (input.MenuOpenClose)
        {
            paused = true;
            canvas.SetActive(true);
            pausemenu.SetActive(true);
            Time.timeScale = 0f;
            input.UIActionMapEnable();
            eventSystem.SetSelectedGameObject(resumebutton);
        }
        else if (input.MenuOpenClose) { //if esc was pressed and cutscene not playing
            paused = true;
            canvas.SetActive(true);
            pausemenu.SetActive(true);
            Time.timeScale = 0f;
            input.UIActionMapEnable();
            eventSystem.SetSelectedGameObject(resumebutton);
            //resumebutton.Select();
        }

    }

    public void MainMenu()
    {
        StartCoroutine(DelayMainMenu());
    }

    public IEnumerator DelayMainMenu()
    {
        playerState.previous_exit = "Dead";
        paused = false;
        canvas.SetActive(false);
        Time.timeScale = 1f;    
        input.inGameActionMapEnable();
        yield return new WaitForSeconds(0.05f);
        save.Save();
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ActivateSettingsMenu(){
        
        // Activate correct menu
        SettingMenu.SetActive(true);
        
        // Deactivate other menus
        ControlMenu.SetActive(false);
        VolumeMenu.SetActive(false);
        pausemenu.SetActive(false);

        // Set selected button
        eventSystem.SetSelectedGameObject(settingsbutton);
    }

    public void ActivatePauseMenu(){

        // Activate correct menu
        pausemenu.SetActive(true);

        // Deactivate other menus
        ControlMenu.SetActive(false);
        VolumeMenu.SetActive(false);
        SettingMenu.SetActive(false);

        // Set selected button
        eventSystem.SetSelectedGameObject(resumebutton);
    }

    public void ActivateControlMenu(){

        // Activate correct menu
        ControlMenu.SetActive(true);

        // Deactivate other menus
        pausemenu.SetActive(false);
        VolumeMenu.SetActive(false);
        SettingMenu.SetActive(false);

        // Set selected button
        eventSystem.SetSelectedGameObject(controlsbutton);
    }

    public void ActivateVolumeMenu(){

        // Activate correct menu
        VolumeMenu.SetActive(true);

        // Deactivate other menus
        pausemenu.SetActive(false);
        ControlMenu.SetActive(false);
        SettingMenu.SetActive(false);

        // Set selected button
        eventSystem.SetSelectedGameObject(VolumeSlider);
    }
}
