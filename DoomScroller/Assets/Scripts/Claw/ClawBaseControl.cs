using System.Collections;
using System.Collections.Generic;
// using UnityEngine.InputSystem;
using UnityEngine;

    [RequireComponent(typeof(Rigidbody2D))]
    public class ClawBaseControl : MonoBehaviour
    {
        // public UserInput input;
        public Camera clawCam;
        public float moveSpeed = 5f; // Speed at which the sprite follows the cursor
        private Rigidbody2D rb;

    public float minY;
        void Start()
        {
            // Get the Rigidbody2D component attached to the sprite
            rb = GetComponent<Rigidbody2D>();

            // if(input == null)
            // {
            //     input = GameObject.Find("InputManager").GetComponent<UserInput>();
            // }
            if(clawCam == null)
            {
                clawCam = GameObject.Find("Claw Camera").GetComponent<Camera>();
            }
        }

    void Update()
    {
        // Get the position of the mouse in the world space
        Vector3 mousePosition = clawCam.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.y < minY )
            mousePosition.y = minY;


        // Calculate direction vector from the current position to the mouse position
        Vector2 direction = (mousePosition - transform.position).normalized;



        // Apply velocity to move the sprite towards the mouse position
        rb.velocity = direction * moveSpeed;
    }
    }

