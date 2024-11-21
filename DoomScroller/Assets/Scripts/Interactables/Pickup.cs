using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public enum types
    {
        hp,
        parry,
        jumps
    }
    public PlayerStateManager player;
    public SaveManager save;
    public int id;
    [SerializeField]
    types type = new types();

    void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player").GetComponent<PlayerStateManager>();
        }
        if (save == null)
        {
            save = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        }

    }

    private void Update()
    {
        if ((int)((player.pickups % Math.Pow(2, id + 1)) / Math.Pow(2, id)) == 1) Destroy(gameObject);
    }

    void collect()
    { 
        player.pickups += Math.Pow(2,id);
        // player.collected(type.ToString());
        Destroy(gameObject);
        save.Save();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            collect();
        }
    }
}
