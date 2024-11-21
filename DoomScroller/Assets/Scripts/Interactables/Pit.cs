using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Pit : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject player;
    public Entity PlayerHealth;
    public GameObject spawnpoint;   
    SpriteRenderer SpriteRenderer;

    //audio effects
    //public EventReference respawn;
    //public EventReference fallsounds;

    //fading effect
    public Canvas PitEffect;
    public CanvasGroup blackcanvasgroup;
    bool fadein = false;
    bool fadeout = false;

    void Start()
    {
        SpriteRenderer = player.GetComponent<SpriteRenderer>();
        blackcanvasgroup = PitEffect.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {

        if (fadein == true)
        {
            blackcanvasgroup.alpha += Time.deltaTime;

            if (blackcanvasgroup.alpha >= 1)
            {
                fadein = false;
                fadeout = true;
            }
        }


        if(fadeout ==true)
        {
            blackcanvasgroup.alpha -= Time.deltaTime;

            if (blackcanvasgroup.alpha <= 0) fadeout = false;
        }
    }

    //on trigger: disables player, then delay spawns player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            player.SetActive(false);
            StartCoroutine(DelaySpawn(1f));

            //play fall sound
            //RuntimeManager.PlayOneShot(fallsounds);
        }
    }

    IEnumerator DelaySpawn(float time)
    {
        //fade to black effect
        fadein = true;
        fadeout = false;

        //delayed spawn
        yield return new WaitForSeconds(time);
        player.SetActive(true);
        player.transform.position = spawnpoint.transform.position;

        //add health damage here
        PlayerHealth.TakeDamage(2);

        //play respawn sound
        //RuntimeManager.PlayOneShot(respawn);

    }

}
