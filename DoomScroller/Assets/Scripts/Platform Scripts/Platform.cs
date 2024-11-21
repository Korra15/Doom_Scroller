using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Platform : MonoBehaviour
{
    public GameObject claw;
    public Sprite sprite;
    public bool isGrabbed;
    public int platformMass ;
    public enum type
    {
        Normal,
        Heavy,
        Jump,
        Sliding
    };

    public abstract void Update();
    

    public virtual void OnGrabbed(GameObject claw)
    {
        this.claw = claw;
        transform.SetParent(claw.gameObject.transform);

        isGrabbed = true;
        
    }

    public virtual void OnDropped()
    {
        transform.SetParent(null);

        isGrabbed = false;
    }
    

    // Start is called before the first frame update
    
}
