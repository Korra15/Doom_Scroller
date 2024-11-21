using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPlatform : Platform
{
    // Start is called before the first frame update
    public override void Update()
    {
      
    }
    public override void OnDropped()
    {

    }
    //On release set weights back up
    public override void OnGrabbed(GameObject claw)
    {
        base.OnGrabbed(claw);
    }
}
