using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onboarding : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    public void StartGame()
    {
        Time.timeScale = 1f;
    }

}
