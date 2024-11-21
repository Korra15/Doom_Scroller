using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;




public class OpenCutsceneToGraveyard : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Canvas canvas;
    public SaveManager save;
    public PlayerStateManager player;

    private bool videoEnded = false;

    public GameObject playerObject;
    public GameObject cutsceneElements;
    public Canvas healthUI;

    void Awake()
    {
        //Debug.Log(player.opening_cutscene_played + "tha shit");
        if(!player.opening_cutscene_played) 
        {
            healthUI.gameObject.SetActive(false);
            videoPlayer.Play();
        }

        // if (player.opening_cutscene_played) cutsceneElements.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnded; // subscribe to end of video
        playerObject.GetComponent<PlayerStateManager>().movementEnabled = false; //disable player movement during the cutscene
        // if(!player.opening_cutscene_played) 
        // {
        //     videoPlayer.Play();
        //     player.opening_cutscene_played = true;
        //     save.Save();
        // }
    }

    // Update is called once per frame
    void Update()
    {
        if ((!videoPlayer.isPlaying && videoEnded) || player.opening_cutscene_played)
        {
            canvas.gameObject.SetActive(false); //hide video end
            healthUI.gameObject.SetActive(true); //hide video end
            playerObject.GetComponent<PlayerStateManager>().movementEnabled = true; // enable player movement on video end
            player.opening_cutscene_played = true;
            //save.Save();
        }
    }

    void OnVideoEnded(VideoPlayer vp)
    {
        // Set the flag indicating that the video has ended
        videoEnded = true;
        player.opening_cutscene_played = true;
        save.Save();
    }
}
