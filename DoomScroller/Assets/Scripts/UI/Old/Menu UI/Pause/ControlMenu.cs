using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class ControlMenu : MonoBehaviour
{
    public GameObject XboxMenu;
    public GameObject PSMenu;
    public GameObject KeyboardMenu;

    public PlayerInput playerInput;

    void Awake(){
        playerInput = GameObject.Find("InputManager").GetComponent<PlayerInput>();
    }

    public void SelectXbox(){
        XboxMenu.SetActive(true);
        PSMenu.SetActive(false);
        KeyboardMenu.SetActive(false);
    }    

    public void SelectPS(){
        XboxMenu.SetActive(false);
        PSMenu.SetActive(true);
        KeyboardMenu.SetActive(false);
    }

    public void SelectKeyboard(){
        XboxMenu.SetActive(false);
        PSMenu.SetActive(false);
        KeyboardMenu.SetActive(true);
    }

    public void OnEnable(){
        string controlScheme = playerInput.currentControlScheme;
        // Debug.Log("Current Control Scheme: " + controlScheme);
        UpdateControlScheme(controlScheme);
    }

    private void UpdateControlScheme(string controlScheme)
    {

        if (controlScheme == "Xbox Controller")
        {
            SelectXbox();
        }
        else if (controlScheme == "PlayStation")
        {
            SelectPS();
        }
        else if(controlScheme == "Keyboard")
        {
            SelectKeyboard();
        }
    }
}
