using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SecondMenu : MonoBehaviour
{
    // On Enable set event system first selected to load button
// On Enable set event system first selected to load button
    public EventSystem eventSystem;
    public GameObject loadButton;
    public GameObject secondMenuButton;

    void Start()
    {
        if (eventSystem == null)
        {
            eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        }
        if (loadButton == null)
        {
            loadButton = GameObject.Find("LoadButton");
        }
        if (secondMenuButton == null)
        {
            secondMenuButton = GameObject.Find("SecondMenuButton");
        }
    }

    void OnEnable()
    {
        eventSystem.SetSelectedGameObject(loadButton);
    }

}
