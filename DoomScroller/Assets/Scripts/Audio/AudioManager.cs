using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
   public static AudioManager instance;

    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource LoopSource;
    public float playerLaserDistance;

    [Header ("Audio Clips")]
    public AudioClip BreakablePlatfrom;
    public AudioClip KeyPickup;
    public AudioClip Laser;
    public AudioClip ClawOpen;
    public AudioClip ClawClose;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public AudioSource getSfXSource()
    {
        return SFXSource;
    }

    public AudioSource getLoopSource()
    {
        return LoopSource;
    }

    public void PlaySFX(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
        //audioSource.PlayOneShot(audioClip);
    }

    public void StopSFX(AudioSource audioSource)
    {
        audioSource.Stop();
    }
}
