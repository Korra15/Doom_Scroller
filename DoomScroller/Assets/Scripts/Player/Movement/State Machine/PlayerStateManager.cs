using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.VFX;

public class PlayerStateManager : MonoBehaviour
{
    // Ability Toggles
    [Header("Ability Toggles")]
    public int current_health;
    public bool jump_ability = true;
    public bool dash_ability = true;
    public bool invis_dash_unlock = false;
    public bool crouch_ability = true;
    public bool sprint_ability = true;
    [Header("collectable progress tracker")]
    public double pickups;

    [Header("Gate Unlocks")]
    public bool spawn_gate = false;

    [Header("Cutscene Manager")]
    public bool opening_cutscene_played = false;
    public string previous_exit = "";

    [Header("Effects")]
    public ParticleSystem groundSparks;
    public ParticleSystem smoke;
    public SmokeHandler smokeSystem;

    // Player variables
    [Header("Player Variables")]
    public float speed = 3f;
    public float sprintSpeed = 1f;
    public float crouchSpeed = 1f;
    public float airBrake = 0f;
    public float slidingPower = 24f;
    public float slidingTime = 5f;
    public float fastFallPower = 8f;

    // Adjust player speed variables
    [Header("Adjustable Variables")]
    public float adjustSpeed = 3f;
    public float adjustSprintSpeed = 1f;
    public float adjustCrouchSpeed = 1f;
    public float jumpingPower = 9f;
    public float adjustRobotJump = 2f;

    // momentum variables<
    [Header("Momentum Variables")]
    public float momentum = 0f;
    public float momentum_cap = 8.0f;
    public float momentum_inc = 1f;

    // dash variables
    [Header("Dash Variables")]
    public bool canDash = true;
    public bool invul = false;
    public bool isDiagonal = false;
    public bool isCrowDashing = false;
    public float dashingPower = 24f;
    public float dashingTime = 0.1f;
    public float dashingCooldown = 1f;
    public float wait_time = 2.0f;
    public float timer = 0.0f;
    public float speedBoost = 1f;
    public float air_dash_count = 0;
    public float air_dash_max = 1;

    public float dash_precision = 0.5f;

    // coyote type variables
    [Header("Coyote Time Variables")]
    public bool isJumping;
    public bool robotJump;
    public bool isDashing;
    private float coyoteTime = 0.115f;
    private float coyoteTimeCounter;
    public float jumpBufferTime = 0.115f;
    private float jumpBufferCounter;

    // slide variables
    [Header("Slide Variables")]
    public Vector2 TempVel;
    public float slideSpeedThisFrame;
    public float slide_timer = 0f;
    public float incomp_slide_timer = 0f;

    // direction variables 
    [Header("Direction Variables")]
    public bool isFacingRight = true;

    // On bouncing platform boolean
    [Header("Bouncing Surface")]
    public bool onBounceSurface = false;
    public object checkedGroundTag;

    // private vars
    private float originalGravity;

    // Input variables
    [Header("Axis Variables")]
    public Vector2 axis;

    // Player components
    [Header("Player Components")]
    public Rigidbody2D rb;
    public LayerMask groundLayer;
    public Transform groundCheck;

    [Header("Animator")]
    public Animator animator;

    // transform variable
    [Header("Transform Variables")]
    public Transform Rotcheck;
    public Transform Offset_Fix;

    // boxcollider variables
    [Header("Player Boxcollider Variables")]
    public BoxCollider2D boxcollider2D;
    public Vector2 Default_Offset = new Vector2(0f, 0f);
    public Vector2 IJJSAC_V = new Vector2(1.6f, 3.2f);
    public Vector2 Slide_V = new Vector2(4.4f, 2.15f);
    public Vector2 Crawl_V = new Vector2(3.5f, 2.15f);
    public Vector2 Crouch_V = new Vector2(1.6f, 2.15f);
    public Vector2 Crouch_V_Offset = new Vector2(0f, -0.5f);
    public Vector2 CrouchD_V = new Vector2(4.4f, 2.15f);
    public Vector2 CrouchD_Offset = new Vector2(0.6f, 0f);
    public Vector2 Dash = new Vector2(4.4f, 1.075f);
    public Vector2 Dash_Offset = new Vector2(0.3f, 0f);


    // Current state and possible states
    public PlayerBaseState currentState;
    public PlayerIdleState IdleState = new PlayerIdleState();
    public PlayerWalkState WalkState = new PlayerWalkState();
    public PlayerSprintState SprintState = new PlayerSprintState();
    public PlayerCrouchState CrouchState = new PlayerCrouchState();
    public PlayerSlideState SlideState = new PlayerSlideState();
    public PlayerJumpState JumpState = new PlayerJumpState();
    public PlayerCrouchDashState CrouchDashState = new PlayerCrouchDashState();
    public PlayerDashState DashState = new PlayerDashState();
    public PlayerCrawlState CrawlState = new PlayerCrawlState();

    //variable for knockback
    [Header("Movement State")]
    public bool movementEnabled = true;

    public float horizontalMovement;

    [Header("File Includes")]
    public SaveManager save;
    private bool hasAutoSaved = false;
    public float autoSaveTime = 10f;

    //For Input System
    [Header("Input System")]
    public UserInput input;

    private bool hasDied = false;

    public bool standingStill = false;

    private TrailRenderer trail;

    // Start is called before the first frame update
    void Start()
    {
        // starting state
        trail = GetComponent<TrailRenderer>();
        trail.enabled = false;
        currentState = IdleState;
        currentState.EnterState(this);

        LoadPlayer();
        // for fast fall
        originalGravity = rb.gravityScale;
    }

    // Switch State Function
    public void SwitchState(PlayerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "MainMenu")
        {
            previous_exit = "Dead";
        }

        if(!trail.enabled) StartCoroutine(ResetTrail());

       

        if(movementEnabled){
        currentState.UpdateState(this); // this does not trigger an error when starting game from main menu
        }
        if(input.MoveInput.x == 0 && input.MoveInput.y == 0) standingStill = true;
        
        //Increases gravity when standing still, when moving it's normal
        //(Used for moving quickly up and down slopes; normally the weak gravity would make the player float a bit when descending)
        if (IsGrounded() && !input.JumpHeld)
        {
            rb.gravityScale = 5;
        } 
        else if (!IsGrounded() && currentState != DashState)
        {
            rb.gravityScale = 5;
        }
        else
        {
            rb.gravityScale = originalGravity;
        }

        if (currentState == IdleState)
        {
            rb.angularDrag = 10000;
        }
        else if (currentState != IdleState || input.Jump || input.JumpHeld || currentState == DashState || currentState == JumpState)
        {
            rb.angularDrag = 0.05f;
            rb.drag = 0;
        }

        axis = input.MoveInput;


        Vector3 localScale = transform.localScale;

        if (movementEnabled)
        {
            if (axis.x > 0 && localScale.x < 0 || axis.x < 0 && localScale.x > 0)
            {
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            if (localScale.y < 0)
            {
                localScale.y *= -1f;
                transform.localScale = localScale;
            }

            SmokeCheck();
            //for DashState
            if (currentState == CrouchDashState && currentState == DashState)
            {
                return;
            }

            if (currentState == DashState){
                isDashing = true;
                rb.gravityScale = 0f;
            } else isDashing = false;

            horizontalMovement = axis.x * speed * sprintSpeed * crouchSpeed * airBrake * speedBoost + (momentum * axis.x);


            GroundSpark();
            CoyoteTime();
            Falling();
            Directional_Check();
            Flip();
        }
    }


    public void SmokeCheck() 
    {
        // triggers on sprint, dash, and jump
        if (currentState == JumpState || 
            currentState == DashState || 
            currentState == SprintState ) {
            smokeSystem.turnOn();
        } else {
            smokeSystem.turnOff();
        }
    }
    public void GroundSpark()
    {
        var emission = groundSparks.emission;
        var smokeEmission = smoke.emission;
        if ((rb.velocity.x > 27 || rb.velocity.x < -27) && IsGrounded() && !isDashing) emission.rateOverTime = 75;
        else if (isDashing) smokeEmission.rateOverTime = 100;
        else
        {
            emission.rateOverTime = 0;
            smokeEmission.rateOverTime = 0;
        }
    }

    public bool IsGrounded()
    {
        var checkedObj = Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundLayer);
        if(checkedObj == null) return false;
        checkedGroundTag = checkedObj.tag;
        return true;//used to be .1f. changed due to increase in player's scale size.
    }

    public bool IsNearGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundLayer);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        currentState.OnCollisionEnter2D(this, collision);
    }

    void FixedUpdate()
    {
        // Call the base class FixedUpdate method
        // base.FixedUpdate();

        if (movementEnabled)
        {
            if (IsGrounded())
            {
                speedBoost = 1f;
            }

            if (currentState == CrouchDashState || currentState == DashState)
            {
                return;
            }
            else if (currentState == SlideState)
            {
                slideSpeedThisFrame = Mathf.Lerp(axis.x * speed * 2, axis.x * speed / 2, slide_timer / slidingTime);
                // Ensure Rigidbody is not static before setting velocity
                if (rb.bodyType != RigidbodyType2D.Static)
                {
                    rb.velocity = new Vector2(slideSpeedThisFrame, rb.velocity.y);
                }
                slide_timer += Time.deltaTime;
            }
            else
            {
                // Ensure Rigidbody is not static before setting velocity
                if (rb.bodyType != RigidbodyType2D.Static)
                {
                    rb.velocity = new Vector2(horizontalMovement, rb.velocity.y);
                }
            }
        }
    }

 
    // getters
    public float GetOriginalGravity()
    {
        return originalGravity;
    }

    void Directional_Check()
    {
        if (axis.x != 0 && axis.y != 0) isDiagonal = true;
        else isDiagonal = false;
    }

    public bool Falling()
    {
        //check if player is falling
        if ((rb.velocity.y < 0) && !IsGrounded())      //removed "&& currentState != JumpState" bc it doesn't work correctly with it. and removed " || rb.velocity.y > 0", not sure why a positive velocity counts as falling. 
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GroundDown()
    {
        //check if player is grounded (not falling) and is inputting down
        if ((axis.y < 0) && !(this.Falling()))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator JumpCooldown()
    {
        isJumping = true;
        yield return new WaitForSeconds(0.4f);
        isJumping = false;
    }

    private void Flip()
    {
        if (!isDashing)
        {
            if (isFacingRight && axis.x < 0f || !isFacingRight && axis.x > 0f)
            {
                Vector3 localScale = transform.localScale;
                isFacingRight = !isFacingRight;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            if (isFacingRight) groundSparks.startRotation = -75 * Mathf.Deg2Rad;
            else if (!isFacingRight) groundSparks.startRotation = 60 * Mathf.Deg2Rad;
        }
    }

    private void CoyoteTime()
    {
        //coyote timing
        if (currentState != JumpState && IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (input.Jump && !isDashing && currentState != DashState)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && !isJumping && !isDashing && currentState != DashState && jump_ability)
        {
            // !!!!!!!!!!!!!!!!!!!!!!!!
            // THIS IS CAUSING PROBLEMS
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            // !!!!!!!!!!!!!!!!!!!!!!!!

            jumpBufferCounter = 0f;

            StartCoroutine(JumpCooldown());
        }

        if (input.JumpReleased && rb.velocity.y > 0f && !isDashing && currentState != DashState && IsGrounded())
        {
            // rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);

            coyoteTimeCounter = 0f;
        }
    }

    public IEnumerator AutoSave()
    {
        hasAutoSaved = true;
        save.Save();

        yield return new WaitForSeconds(autoSaveTime);
        hasAutoSaved = false;
    }

        public void VerticalFlip()
    {
        Vector3 localScale = transform.localScale;
        if (transform.rotation.eulerAngles.z > 135f && transform.rotation.eulerAngles.z < 225f)
        {
            if (localScale.y > 0)
            {
                localScale.y *= -1f;
                transform.localScale = localScale;
            }
        }
        else if (transform.rotation.eulerAngles.z > -45f && transform.rotation.eulerAngles.z < 45f)
        {
            if (localScale.y < 0)
            {
                localScale.y *= -1f;
                transform.localScale = localScale;
            }
        }
    }

    public IEnumerator ResetTrail()
    {
        yield return new WaitForSeconds(0.5f);
        trail.enabled = true;
    }

    public void LoadPlayer()
    {
        GameData data = save.LoadPlayerData();
        
        if (data != null ) { 
        this.pickups = data.pickups;

        this.jump_ability = true;
        this.dash_ability = true;
        this.invis_dash_unlock = data.invis_dash_unlock;
        this.crouch_ability = data.crouch_ability;
        this.sprint_ability = data.sprint_ability;

        this.opening_cutscene_played = data.opening_cutscene_played;
        this.spawn_gate = data.spawn_gate;}
    }

    string CurrentAnimationName = "";
}
