using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [Header("Transform Variables")]
    public Transform target;
    private Transform boss;

    [Header("Vector3 Variables")]
    public Vector3 Middle;
    public Vector3 offset;
    public Vector3 bossOffset;
    public Vector3 barrierPositionLeft;
    public Vector3 barrierPositionRight;


    [Header("Float Variables")]
    public float damping;

    [Header("Boolean Variables")]
    public bool bossMode = false;

    private Vector3 velocity = Vector3.zero;
    Vector3 movePosition;
    private int lockX;

    [Header("Barriers")]

    public GameObject leftBarrier;
    public GameObject rightBarrier;

    [Header("Player Rigid Body Variables")]
    public Rigidbody2D rb;

    [Header("Camera Distance Variables")]
    public float x_ratio = 0.5f;
    public float initialXOffset = 0f;
    // For this one how much of the y velocity do you want applying to distance
    public float xGoal;
    public float y_ratio = 0.5f;
    public float initialYOffset = 1.4f;
    // TBH I multiply this by Time.deltaTime and shit happens
    public float yGoal;
    public float smoothness = 0.5f;
    public float speedLimit = 6f;
    // Default zoom out amount is -13(offset.z) so make it more negative to zoom out
    // If the default changes in the future just make it more negative than default 
    // <3
    public float zoomOutAmount = -20f;
    public float defaultZoomOut = -17f;

    private bool standing;

    

    [Header("Look Up n Down Variales")]
    public UserInput input;
    public float lookUpAmount = 0.5f;
    public float lookDownAmount = 0.5f;
    // Keep the y offset same as initialYOffset
    public float keepYOffset = 1.4f; 
    public float lookSpeed = 1f;

    void Awake(){
        keepYOffset = initialYOffset;
    }


    void Update() {
        standing = (rb.velocity.x == 0 && rb.velocity.y == 0);
        if (lockX == 0) {
            SmoothCameraMovement();
            CameraLook();
        }
        if(!standing){
            initialYOffset = keepYOffset;
        }
    }

    void CameraLook(){
        if(!standing) return;
        if(input.MoveInput.y > 0){
            initialYOffset = keepYOffset + lookUpAmount;
        }else if(input.MoveInput.y < 0){
            initialYOffset = keepYOffset - lookDownAmount;
        }else{
            initialYOffset = keepYOffset;
        }
    }

    void SmoothCameraMovement()
    {   
        if(standing){
            if(Mathf.Abs(initialXOffset - offset.x) > 0.1f){
                offset.x = initialXOffset;
            }else{
                offset.x =  Mathf.Lerp(offset.x,initialXOffset, (Time.deltaTime * smoothness));
            }
           
            if(Mathf.Abs(initialYOffset - offset.y) > 0.1f){
                offset.y =  Mathf.Lerp(offset.y,initialYOffset, (Time.deltaTime * smoothness) * lookSpeed);
            }else{
                offset.y =  Mathf.Lerp(offset.y,initialYOffset, (Time.deltaTime * smoothness) * lookSpeed);
            }
            
        }else{
            offset.x =  Mathf.Lerp(offset.x,(rb.velocity.x * x_ratio), Time.deltaTime * smoothness);
            offset.y =  Mathf.Lerp(offset.y,initialYOffset + (rb.velocity.y * y_ratio), Time.deltaTime * smoothness);
        }

        if(Mathf.Abs(rb.velocity.x) > speedLimit || Mathf.Abs(rb.velocity.y) > speedLimit){
            offset.z = Mathf.Lerp(offset.z,zoomOutAmount, Time.deltaTime * smoothness);
        }else{
            offset.z = Mathf.Lerp(offset.z,defaultZoomOut, Time.deltaTime * smoothness);
        }
        //Debug.Log(rb.velocity);
    }

    void LateUpdate()
    {
        if(!bossMode)
        {
            movePosition = target.position + offset;
            barrierPositionLeft = new Vector3(leftBarrier.transform.position.x, target.position.y + offset.y, offset.z);
            barrierPositionRight = new Vector3(rightBarrier.transform.position.x, target.position.y + offset.y, offset.z);
        }
        else
        {
            movePosition = Middle + bossOffset;
        }
        if (target.transform.position.x > leftBarrier.transform.position.x && target.transform.position.x < rightBarrier.transform.position.x) {
            transform.position = Vector3.SmoothDamp(transform.position, movePosition, ref velocity, damping);
            lockX = 0;
        }
        else if (target.transform.position.x <= leftBarrier.transform.position.x){ 
            transform.position = Vector3.SmoothDamp(transform.position, barrierPositionLeft, ref velocity, damping);
            lockX = 1; 

        }
        else if (target.transform.position.x >= rightBarrier.transform.position.x){ 
            transform.position = Vector3.SmoothDamp(transform.position, barrierPositionRight, ref velocity, damping);   
        }
    }


}
