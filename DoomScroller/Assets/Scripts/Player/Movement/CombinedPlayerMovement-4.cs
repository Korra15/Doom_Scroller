using System.Collections;
using UnityEngine;
using System;

public class CombinedPlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    public float speed = 3f;
    public float sprintSpeed = 6f;
    public float jumpingPower = 8f;
    public float airBrake = 1;

    private bool isFacingRight = true;

    private bool canDash = true;
    private bool isDashing;

    public float dashingPower = 1f;
    public float dashingTime = 0f;
    public float dashingCooldown = 0f;

    //Start of Jennifer stuff
    private bool isJumping;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;
    //End of Jennifer stuff


    //start of David stuff
    private bool crouching = false;
    private float crouchSpeed = 1f;
    private bool sliding = false;
    private bool canSlide = true;
    private bool sprinting = false; // for sliding
    public bool isDiagonal = false;
    Vector3 OGScale; // for crouching
    public float slidingPower = 24f;
    public float slidingTime = 0.1f;
    public float slideCooldown = 0f;
    float OGGravity;
    //end of David stuff   

    //start of Chimal stuff
    public float dash_multi = 1f;
    public float multi_cap= 2.0f;
    public float wait_time = 2.0f;
    public float timer = 0.0f;
    public bool invis_dash_unlock = false;
   
    //end of Chimal stuff

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        OGScale = transform.localScale;
        OGGravity = rb.gravityScale;
    }

    void Update()
    {
        //Start of Chimal
        if(rb.velocity.x == 0f ){
            if(rb.velocity.y == 0f){
                timer += Time.deltaTime;
            }else{
                timer = 0f;

            }
        }else{
            timer = 0f;

        }
        //End of Chimal
        if (isDashing)
        {
            return;
        }

        //Start of Jennifer stuff

        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && !isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);

            jumpBufferCounter = 0f;

            StartCoroutine(JumpCooldown());
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

            coyoteTimeCounter = 0f;
        }
        /*
        if (Input.GetKeyDown(KeyCode.I) && canDash)
        {
            Debug.Log("Invincible Dash");
            StartCoroutine(InvincibleDash());
        }
        */
        //End of Jennifer stuff
        
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.Space) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.J) && canDash)
        {
            if(invis_dash_unlock){
                if(GlobalControl.DebugEnabled) Debug.Log("Invincible Dash");
                StartCoroutine(InvincibleDash());

            }else{
                if(GlobalControl.DebugEnabled) Debug.Log("Dash");
                StartCoroutine(Dash());
            }
        }
        
        Crouch(); //added by David
        AirControl();
        Sprint();
        Directional_Check();

        SpeedBoost();
        //Flip();
    }

    private void FixedUpdate()
    {
        if (isDashing || sliding)
        {
            return;
        }

        rb.velocity = new Vector2(horizontal * speed * airBrake * sprintSpeed * crouchSpeed * dash_multi, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }
    private bool IsUnder()
    {
        return Physics2D.OverlapCircle(ceilingCheck.position, 0.1f, groundLayer);
    }

    //Start of David stuff
    void Crouch()
    {
        // ground + LCtrl input check
        if (IsGrounded() && !crouching && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
        {
            // crouch
            transform.localScale = new Vector3(OGScale.x, OGScale.y * 0.5f, OGScale.z);
            crouchSpeed = 0.5f;
            crouching = true;

            // checking if sprinting while attempting to crouch
            if (sprinting && canSlide ) // slide
            {
                StartCoroutine(Slide());
            }

        }

        // get out of crouch
        if (!(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && crouching /*&& !IsUnder()*/)
        {
            transform.localScale = OGScale;
            crouchSpeed = 1f;
            crouching = false;
        }

        
        // speed up falling
        if (!IsGrounded() && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
        {
            rb.gravityScale = 10f;
        } 
        if(IsGrounded())
        {
            rb.gravityScale = OGGravity;
        }
    }

    private IEnumerator Slide()
    {


        canSlide = false;
        sliding = true;
        float originalGravity = rb.gravityScale;
        float tempslidingtime = slidingTime;
        rb.gravityScale = rb.gravityScale * 0.5f;
        float timer = 0f;
        Vector2 TempVel = rb.velocity;
        Vector2 slideDirection = new Vector2(horizontal, 0f);
        if(horizontal < 0){
            while (timer < slidingTime)
            {
                crouchSpeed = 1.5f * -1f;
                float slideSpeedThisFrame = Mathf.Lerp(rb.velocity.x + (slidingPower)*-1f, crouchSpeed, timer / slidingTime);
                //Debug.Log("crch:"+crouchSpeed);
                //Debug.Log("vel:"+rb.velocity.x);
                //Debug.Log("slpow:"+slidingPower * -1f);
                rb.velocity = new Vector2(slideSpeedThisFrame,0f);
                timer += Time.deltaTime;
                if(!crouching){
                    slidingTime = 0;
                    rb.velocity = TempVel;
                }
                yield return null; // Wait for the next frame.
            }
        }else{
             while (timer < slidingTime)
            {
                crouchSpeed = 1.5f;
                float slideSpeedThisFrame = Mathf.Lerp(rb.velocity.x + (slidingPower), crouchSpeed, timer / slidingTime);
                rb.velocity = slideDirection * slideSpeedThisFrame;
                timer += Time.deltaTime;
                if(!crouching){
                    slidingTime = 0;
                    rb.velocity = TempVel;
                }
                yield return null; // Wait for the next frame.
            }
        }
        crouchSpeed = 0.5f;
        yield return new WaitForSeconds(slidingTime); 
        rb.gravityScale = originalGravity;
        sliding = false;
        yield return new WaitForSeconds(slideCooldown);
        canSlide = true;
        slidingTime = tempslidingtime;
    }
    //End of David stuff

    void AirControl()
    {
        if (!IsGrounded() && rb.velocity.x >= 0 && horizontal <= 0) airBrake = 0.5f;
        else if (!IsGrounded() && rb.velocity.x <= 0 && horizontal >= 0) airBrake = 0.5f;
        else if (isDashing) airBrake = 1;
        else if (IsGrounded()) airBrake = 1;
    }

    void Sprint()
    {
        // slide from crouch
        /*
        if(Input.GetKeyDown("left shift") && !crouching && canSlide)
        {
            StartCoroutine(Slide());
            return;
        }
        No slide from crouch
        */ 

        if (Input.GetKey("left shift") && IsGrounded() && !crouching)
        {
            sprintSpeed = 2;
            sprinting = true; // for sliding
        }
        else if (Input.GetKey("left shift") && !IsGrounded() && Mathf.Abs(rb.velocity.x) > 3)
        {
            sprintSpeed = 2;
        }
        else
        {
            sprintSpeed = 1;
            sprinting = false; // for sliding
        }
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    
    void Directional_Check()
    {
        if (((rb.velocity.x > 0 || rb.velocity.x < 0) && rb.velocity.y > 0 ) || rb.velocity.y > 0) isDiagonal = true;
        else isDiagonal = false;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        if (horizontal > 0 || horizontal < 0){
            rb.velocity = new Vector2(horizontal * dashingPower, vertical * dashingPower * 0.7f);
            dash_multi = dash_multi + 0.3f;
        }else if (vertical > 0 || vertical < 0){
            rb.velocity = new Vector2(0, vertical * dashingPower);
            dash_multi = dash_multi + 0.3f;
        }
        yield return new WaitForSeconds(dashingTime);
        if (isDiagonal) rb.velocity = new Vector2(horizontal * speed * sprintSpeed, dashingPower / 3);
        else rb.velocity = new Vector2(horizontal * speed * sprintSpeed, 0);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    //Start of Jennifer Stuff
    private IEnumerator JumpCooldown()
    {
        isJumping = true;
        yield return new WaitForSeconds(0.4f);
        isJumping = false;
    }

    private IEnumerator InvincibleDash()
    {
        canDash = false;
        isDashing = true;
        Physics2D.IgnoreLayerCollision(0,6,true);
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        if (horizontal > 0 || horizontal < 0){
            rb.velocity = new Vector2(horizontal * dashingPower, vertical * dashingPower * 0.7f);
            //Start of Chimal
            dash_multi = dash_multi + 0.3f;
            //End of Chimal
        }
        else if (vertical > 0 || vertical < 0){
            rb.velocity = new Vector2(0, vertical * dashingPower);
            //Start of Chimal
            dash_multi = dash_multi + 0.3f;
            //End of Chimal
        }
        yield return new WaitForSeconds(dashingTime);
        if (isDiagonal) rb.velocity = new Vector2(horizontal * speed * sprintSpeed, dashingPower / 3);
        else rb.velocity = new Vector2(horizontal * speed * sprintSpeed, 0);
        rb.gravityScale = originalGravity;
        isDashing = false;
        Physics2D.IgnoreLayerCollision(0,6,false);
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    // private void OnCollisionEnter2D () 
    // {
    //     if (isDashing) {
    //         Physics2D.IgnoreLayerCollision(0,6,true);
    //     }
    //     if (isDashing == false) {
    //         Physics2D.IgnoreLayerCollision(0,6,false);
    //     }
    // }    
    //End of Jennifer Stuff

    //Start of Chimal Stuff
    void SpeedBoost()
    {
        if(timer > wait_time){
            timer = 0f;
            dash_multi = 1f;
            //Debug.Log("Reset Dash Multi Timer");
        }

        if(dash_multi > multi_cap){
           dash_multi = 1.9f;
           //Debug.Log("Reduce multi");
        }

        if(dash_multi >= 1f && rb.velocity.x >= 0f && horizontal <= 0){
            dash_multi = 1f;
        }

        if(dash_multi >= 1f && rb.velocity.x <= 0f && horizontal >= 0){
            dash_multi = 1f;
        }
    }
    //End of Chimal Stuff
}
