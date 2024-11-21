using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockAndKey : Platform
{
    public bool isOpen = false;
    public Sprite openSprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
    }


    public override void OnGrabbed(GameObject claw)
    {
        if(isOpen)
        base.OnGrabbed(claw);
    }
    public override void OnDropped()
    {
        if(isOpen)
        base.OnDropped();
    }
    public void SetOpen()
    {
        isOpen = true;
        GetComponent<SpriteRenderer>().sprite = openSprite;
    }


}
