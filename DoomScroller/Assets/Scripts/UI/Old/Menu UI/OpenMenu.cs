using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OpenMenu : MonoBehaviour
{

    public GameObject mainMenu;
    public GameObject secondMenu;
    public GameObject settingsMenu;
    public EventSystem eventSystem;
    public GameObject SecondMenuButton;
    public GameObject VolumeButton;
    public GameObject loadButton;
    
    public void ActivateSecondMenu() {
        if (secondMenu != null) secondMenu.SetActive(true);
        if (mainMenu != null) mainMenu.SetActive(false);
        if (settingsMenu != null) settingsMenu.SetActive(false); 
        eventSystem.SetSelectedGameObject(loadButton);

    }

    public void ActivateMainMenu() {
        if (secondMenu != null) secondMenu.SetActive(false);
        if (mainMenu != null) mainMenu.SetActive(true);
        if (settingsMenu != null) settingsMenu.SetActive(false);
        eventSystem.SetSelectedGameObject(SecondMenuButton);
    }

    public void ActivateSettingsMenu() {
        if (secondMenu != null) secondMenu.SetActive(false);
        if (mainMenu != null) mainMenu.SetActive(false);
        if (settingsMenu != null) settingsMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(VolumeButton);
    }
    
}
