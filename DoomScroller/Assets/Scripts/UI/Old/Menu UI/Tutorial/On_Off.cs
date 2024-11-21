using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class on_off : MonoBehaviour
{
    public bool Activate;
    private bool once = true;

    public controlsTutorial tutorial;

    // On Trigger based on on_off flip tutorial.on_off
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the trigger has a specific tag
        if (other.CompareTag("Player"))
        {
            if(once){
                once = false;
                if(Activate){
                    tutorial.on_off = true;
                }else{
                    tutorial.on_off = false;
                }
            }
        }
    }
}
