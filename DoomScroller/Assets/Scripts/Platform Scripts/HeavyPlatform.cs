using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyPlatform : Platform
{
    public Transform clawPos;
    public Vector3 claw_heavy_loaction_fix;
    Rigidbody2D rigidbody2D;
  
 
    // Start is called before the first frame update
    void Start()
    {
           rigidbody2D = gameObject.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    public override void Update()
    {
       // if (isGrabbed)
           // this.transform.position = clawPos.position + claw_heavy_loaction_fix;
    }
    //On grab, set weight low
    public override void OnDropped()
    {
        base.OnDropped();
        rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
        rigidbody2D.mass = 50;
        //rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        Rigidbody2D clawRigid = claw.GetComponent<Rigidbody2D>();
        rigidbody2D.AddForce(new Vector2(clawRigid.velocityX, clawRigid.velocityY)*(1000));
        rigidbody2D.AddTorque(clawRigid.angularVelocity );
        isGrabbed = false;
    }
    //On release set weights back up
    public override void OnGrabbed(GameObject claw)
    {
        base.OnGrabbed(claw);
        Destroy(rigidbody2D);

       // this.clawPos = claw.transform;
      //  claw_heavy_loaction_fix = this.transform.position - clawPos.position;
       // base.OnGrabbed( claw);
       // isGrabbed = true;
       // rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
    }
}
