using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import the UI namespace

public class SettingManager : MonoBehaviour
{
    public float MasterVolume;
    public float BackGroundMusic;
    public float SFX;

    public SaveManager save;

    public void Start(){
        save = GameObject.Find("SaveManager").GetComponent<SaveManager>();

        loadVolumes();
    }

    public void loadVolumes(){

        SettingData data = save.LoadSettings();
        MasterVolume = data.MasterVolume;
        BackGroundMusic = data.BackGroundMusic;
        SFX = data.SFX;
    }

    public void OnDestroy()
    {
        save.saveSettings();
        SettingData data = save.LoadSettings();
        //Debug.Log("Saved volumes are | Master: " + data.MasterVolume + " | Music: " + data.BackGroundMusic + " | SFX: " + data.SFX);
    }

    public void saveSettings(){
        save.saveSettings();
    }
}
