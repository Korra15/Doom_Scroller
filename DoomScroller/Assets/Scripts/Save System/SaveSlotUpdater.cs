using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSlotUpdater : MonoBehaviour
{
    // This is a function for the Main Menu Scene. 
    // It just updates the UI for the save slots on wheter the file exists or not.
    // Beep Boop no exist poof
    // :)
    public int SaveId;

    public GameObject SaveManager;
    public GameObject LoadButton;
    public GameObject DeleteButton;
    public GameObject NewSaveButton;

    void Update(){
        if(SaveManager.GetComponent<SaveManager>().FileExists(SaveId)){
            LoadButton.SetActive(true);
            DeleteButton.SetActive(true);
            NewSaveButton.SetActive(false);
        }else{
            LoadButton.SetActive(false);
            DeleteButton.SetActive(false);
            NewSaveButton.SetActive(true);
        }
    }
}
