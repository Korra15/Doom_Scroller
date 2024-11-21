using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour
{
    [SerializeField] private SpriteRenderer clawSpriteRenderer;
    [SerializeField] private Sprite openClaw;
    [SerializeField] private Sprite closeClaw;

    private enum PlayerState
    {
        Free,
        Grabbing,
        Grab // New state for when the player first clicks the mouse
    }

    private PlayerState currentState;
    private GameObject overlappingGrabbableObject;
    private GameObject grabbedObject;
    public float minHieght = -3;
    public int baseMass = 15;

    // Start is called before the first frame update
    void Start()
    {
        currentState = PlayerState.Free;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if right mouse button is first clicked
        if (Input.GetMouseButtonDown(0) && currentState != PlayerState.Grabbing) // Right mouse button pressed down
        {
            if(overlappingGrabbableObject)
            {          
                ChangeState(PlayerState.Grab);
            }

            //Here I will add a short cool down
        }
        // Check if left mouse button is held down
        else if (Input.GetMouseButtonUp(0) && currentState == PlayerState.Grabbing)
        {
            ChangeState(PlayerState.Free);

        }
    }

    private void ChangeState(PlayerState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;

            if (newState == PlayerState.Grab)
            {
                OnGrab();
            }
            else if (newState == PlayerState.Grabbing)
            {
             //nothing here for now
            }
            else if (newState == PlayerState.Free)
            {
                OnFree();
            }
        }
    }

    private void OnGrab()
    {
        Debug.Log("Player has grabbed (right-click).");
        //Grab object, and change state to grabbing
        AudioManager.instance.PlaySFX(AudioManager.instance.getSfXSource(), AudioManager.instance.ClawClose);

        clawSpriteRenderer.sprite = closeClaw;


        grabbedObject = overlappingGrabbableObject;
        ChangeState(PlayerState.Grabbing);
        overlappingGrabbableObject.GetComponent<Platform>().OnGrabbed(this.gameObject);
        GetComponent<Rigidbody2D>().mass = baseMass + overlappingGrabbableObject.GetComponent<Platform>().platformMass;
    }

    

    private void OnFree()
    {
        Debug.Log("object is now free.");
        // Add behavior for the free state
      
        AudioManager.instance.PlaySFX(AudioManager.instance.getSfXSource(), AudioManager.instance.ClawOpen);
        clawSpriteRenderer.sprite = openClaw;

        grabbedObject.transform.SetParent(null);
        grabbedObject.GetComponent<Platform>().OnDropped();
        grabbedObject = null;
        GetComponent<Rigidbody2D>().mass = baseMass;
    }
    
        
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Grabbable"))
        {
            overlappingGrabbableObject = other.gameObject; // Store reference to the grabbable object
            Debug.Log("Entered grabbable object area: " + other.gameObject.name);
        }
    }

    // This method detects if the player exits a trigger collider
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Grabbable")
        {
            if (overlappingGrabbableObject == other.gameObject)
            {
                overlappingGrabbableObject = null; // Clear reference when leaving the object
                Debug.Log("Exited grabbable object area: " + other.gameObject.name);
            }
        }
    }
}
