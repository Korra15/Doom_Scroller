using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using FMOD.Studio;

public class PlayerAudio : MonoBehaviour
{
    //public AudioSource audio;
    public static PlayerAudio instance { get; private set; }

    private Dictionary<EventReference, EventInstance> eventInstanceDict;
    private List<EventReference> activeLoopingEvents;

    [Header("Movement")]
    public EventReference dash; 
    public EventReference glide; 
    //public EventReference glideStart;
    public EventReference crouchJump;
    public EventReference jump;
    public EventReference footsteps;
    public EventReference jogging;
    //public EventReference running;

    [Header("Offense")]
    public EventReference swordSwing; 
    public EventReference swordHit;

    [Header("Defense")]
    //public EventReference block; 
    public EventReference parry;
    public EventReference unblockableWarning;
    public EventReference shieldHit;
    public EventReference guardHold;


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
        //    Debug.LogError("Found more than one Player Audio in the scene.");
        //}
        //instance = this;
        
        
        eventInstanceDict = new Dictionary<EventReference, EventInstance>();
        activeLoopingEvents = new List<EventReference>();

        SceneManager.sceneUnloaded += HandleSceneUnloaded;
    }


    private void HandleSceneUnloaded(Scene scene)
    {
        foreach(EventReference eventReference in activeLoopingEvents)
        {
            StopSFXLoop(eventReference);
        }
    }

    public void PlayOneShot(EventReference eventReference)
    {
        RuntimeManager.PlayOneShot(eventReference);
    }

    public void PlaySFXLoop(EventReference eventReference)
    {
        if (!eventInstanceDict.ContainsKey(eventReference))
        {
            eventInstanceDict.Add(eventReference, RuntimeManager.CreateInstance(eventReference));
            activeLoopingEvents.Add(eventReference);
            eventInstanceDict[eventReference].start();
        }
        
    }

    public void StopSFXLoop(EventReference eventReference)
    {
        if (eventInstanceDict.ContainsKey(eventReference)) //checking if the eventInstance was assigned. If so, stop the eventInstance
        {
            eventInstanceDict[eventReference].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            eventInstanceDict[eventReference].release();
            eventInstanceDict.Remove(eventReference);
            activeLoopingEvents.Remove(eventReference);
        }
    }
}
