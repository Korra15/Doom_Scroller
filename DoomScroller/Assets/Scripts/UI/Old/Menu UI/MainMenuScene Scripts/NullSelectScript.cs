using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class NullSelectScript : MonoBehaviour
{
    // Update is called once per frame
    public GameObject previous;
    
    public EventSystem eventSystem;

    void Start(){
        if(eventSystem == null){
            eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
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
