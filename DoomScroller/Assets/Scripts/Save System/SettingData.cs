using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingData
{

    public float MasterVolume;
    public float BackGroundMusic;
    public float SFX;


    public SettingData(SettingManager manager){
        this.MasterVolume = manager.MasterVolume;
        this.BackGroundMusic = manager.BackGroundMusic;
        this.SFX = manager.SFX;
    }

    public SettingData(){
        this.MasterVolume = 1.0f;
        this.BackGroundMusic = 1.0f;
        this.SFX = 1.0f;
    }
    
}
