using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public float pointIncreasedPerSecond = 1f;
    public Main_Menu menu;

    private void FixedUpdate()
    {
        if (menu.isPaused ==  false)
        {
            PersistentGameData.score += pointIncreasedPerSecond * Time.fixedDeltaTime;
            //Debug.Log(PersistentGameData.score);    
        }
        //Debug.Log(PersistentGameData.score);    
    }


}
