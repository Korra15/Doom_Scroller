using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class NullSelectPause : MonoBehaviour
{

    // Update is called once per frame
    public GameObject previous;
    
    public EventSystem eventSystem;

    void Awake(){
        if(eventSystem == null){
            eventSystem = GameObject.Find("InputManager").GetComponent<EventSystem>();
        }  
    }

    void Update()
    {
        GameObject current = eventSystem.currentSelectedGameObject;
        if (current != previous){
            if(current == null){
                eventSystem.SetSelectedGameObject(previous);
            } else {
                previous = current;
            }
        }
    }
}
