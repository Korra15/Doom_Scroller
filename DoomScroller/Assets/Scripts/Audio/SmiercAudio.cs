using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class SmiercAudio : MonoBehaviour
{
    //public AudioSource audio;
    public static SmiercAudio instance { get; private set; }

    public bool sfxPlayed = false;
    
    [Header("Phase 1")]
    public EventReference chargeAndSweep; 
    public EventReference horseStomp; 
    public EventReference rifleBlast; 
    public EventReference scytheLightAttack; 
    public EventReference unblockableWaveBlast; 

    [Header("Phase 2")]
    public EventReference floatingTree; 
    public EventReference galloping; 
    public EventReference growingTree; 
    public EventReference horseKickback; 
    public EventReference lockonShooting; 
    public EventReference thornBush; 
    public EventReference waveKnockback;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //if (instance != null)
        //{
        //    Debug.LogError("Found more than one Smierc Audio in the scene.");
        //}
        //instance = this;
    }


    public IEnumerator PlayOneShot(EventReference eventReference)
    {
        sfxPlayed = true;
        RuntimeManager.PlayOneShot(eventReference);
        yield return new WaitForSeconds(0.15f);
        sfxPlayed = false;
    }
}
