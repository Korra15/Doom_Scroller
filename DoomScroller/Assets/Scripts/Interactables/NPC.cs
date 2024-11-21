using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using TMPro;
using System;

public class NPC : MonoBehaviour
{
    public GameObject player;
    public PlayerStateManager playerStateManager;
    public Entity health;
    //public GameObject returnButton;         //not sure what this is
    public SaveManager save;
    public GameOverLogic gameoverlogic;
    public UserInput input;
    public GameObject sit;
    public GameObject talkUI;   //pop up text

    public bool canTalk = false;
    public bool talking = false;
    public int objTracked = -1;
    public string whatToSay = "Insert Quest Here";
    public string alternativeSay = "Insert Quest response";
    public string greeting = "E to Talk";
    private TextMeshProUGUI textVoice;

    private void Start()
    {
        textVoice = talkUI.GetComponentInChildren<TextMeshProUGUI>();
        print(textVoice);
    }

    private void Update()
    {
        //if button is pressed, make the player talk/stop at npc based on bool canTalk
        if (input.Interact && canTalk)
        {
            if (!talking)
            {
                Debug.Log("entering rest");
                talk();
            }

            else if (talking)
            {
                Debug.Log("exiting rest");
                stopTalk();
            }
        }

        //exit talk using horizontal movement input
        if (input.MoveInput.x != 0 && canTalk)
        {
            if (talking)
            {
                Debug.Log("exiting rest");
                stopTalk();
            }
        }
    }

    //determines if player is on the spawnpoint
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            canTalk = true;
            talkUI.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            canTalk = false;
            talkUI.SetActive(false);
    }

    //saves rest point coordinates and refills HP
    public void talk()
    {
        talking = true;
        playerStateManager.movementEnabled = false;
        player.transform.position = new Vector2(this.transform.position.x,player.transform.position.y);

        //returnButton.SetActive(true);

        //makes player sit
        player.SetActive(false);
        sit.SetActive(true);
        canTalk = true;     //makes sure player can unrest, since disabling the player gameobject triggers OnTriggerExit2d()
        print(textVoice);
        textVoice.text = whatToSay;
        if (objTracked != -1)
        {
            if (((int)((playerStateManager.pickups % Math.Pow(2, objTracked + 1)) / Math.Pow(2, objTracked)) == 1))
            {
                textVoice.text = alternativeSay;
            }
        }
        talkUI.SetActive(true);
    }

    //leave rest point
    public void stopTalk()
    {
        talking = false;
        playerStateManager.movementEnabled = true;
        //returnButton.SetActive(false);

        player.SetActive(true);
        sit.SetActive(false);
        textVoice.text = greeting;
    }
    
}
