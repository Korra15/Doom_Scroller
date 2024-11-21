using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;

public class SliderManager : MonoBehaviour
{
    public SettingManager setting;

    public Slider SMasterVolume;
    public Slider SMusicVolume;
    public Slider SSFXVolume;

    //EventInstance SFXSliderTestAudio

    FMOD.Studio.Bus Master;
    FMOD.Studio.Bus Music;
    FMOD.Studio.Bus SFX;
    //FMOD.Studio.Bus Ambience;

    void Start()
    {
        setting = GameObject.Find("SettingManager").GetComponent<SettingManager>();

        Master = FMODUnity.RuntimeManager.GetBus("bus:/");
        Music = FMODUnity.RuntimeManager.GetBus("bus:/Music");
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/SFX");
        setSliders();
    }

    void Update()
    {
        Master.setVolume(setting.MasterVolume);
        Music.setVolume(setting.BackGroundMusic);
        SFX.setVolume(setting.SFX);
    }

    public void setSliders()
    {
        SMasterVolume.value = setting.MasterVolume;
        SMusicVolume.value = setting.BackGroundMusic;
        SSFXVolume.value = setting.SFX;
    }

    //---AUDIO SLIDER METHODS---//
    public void SetMasterVolume (float newMasterVolume)
    {
        setting.MasterVolume = newMasterVolume;
        setting.saveSettings();
    }

    public void SetMusicVolume (float newMusicVolume)
    {
        setting.BackGroundMusic = newMusicVolume;
        setting.saveSettings();
    }

    public void SetSFXVolume (float newSFXVolume)
    {
        setting.SFX = newSFXVolume;
        setting.saveSettings();
    }
}
