using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private bool isAlreadyInAir = false;
    
    //public PlayerAudio audio;
    public override void EnterState(PlayerStateManager player){
        player.invul = false;


        player.boxcollider2D.size = player.IJJSAC_V;
        player.boxcollider2D.offset = player.Default_Offset;

        // Reset Rotation
        player.Rotcheck.localRotation = Quaternion.Euler(0,0,0);
        
    }

    public override void UpdateState(PlayerStateManager player){
        
        //changes to Falling animation when falling
        if (player.Falling())

        /*
        //slope case
        if (player.IsGrounded()) {
            //player.air_dash_count = 0;
            //player.rb.gravityScale = player.GetOriginalGravity();
            player.SwitchState(player.WalkState);
        }
        */
        if (player.IsNearGround()) {
            player.robotJump = false;
        }
        if ((player.input.JumpReleased || !player.input.JumpHeld) && player.robotJump == false && !player.Falling() && (player.rb.velocity.y > player.jumpingPower/2)) {
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpingPower / player.adjustRobotJump);
            player.robotJump = true;
        }
        if (player.IsGrounded() && !player.isJumping && !player.input.JumpHeld) { // On ground, not jumping, jump key isn't being held
            player.robotJump = false;
            player.air_dash_count = 0;
            isAlreadyInAir = false;
            player.SwitchState(player.IdleState);
        }
        else
        {
            player.rb.drag = 0;
        }

    

        if (player.rb.velocity.x >= 0 && !player.isFacingRight){
            player.sprintSpeed = 1f;
        }

        // Enable Air Breaks if Going Left
        if (player.rb.velocity.x <= 0 && player.isFacingRight){
            player.sprintSpeed = 1f;
        }

   

        // Reset Momentum in the Air
         if(player.momentum >= 0f && player.rb.velocity.x >= 0f && player.axis.x <= 0){
            player.momentum = 0f;
            isAlreadyInAir = true;
        }
        
        // Reset Momentum in the Air
        if(player.momentum >= 0f && player.rb.velocity.x <= 0f && player.axis.x >= 0){
            player.momentum = 0f;
            isAlreadyInAir = true;
        }

        // Might fix the air control glitch?
        if(player.input.CrouchHeld && player.IsGrounded() && !player.isJumping && player.crouch_ability) { // added && isJumping to make crouch -> jumping break out of crouch animation
            player.SwitchState(player.CrouchState);
            player.air_dash_count = 0;
        }
        if(player.input.CrouchReleased) { // if the player crouch jumps but then lets go of crouch mid-jump, it should now break out of crouch state and go to normal mov speed
            player.SwitchState(player.WalkState);
        }

        // Dash Once in Air
        if(player.input.Dash && player.canDash && player.air_dash_count < player.air_dash_max && player.dash_ability){
            player.air_dash_count = 0;
            player.SwitchState(player.DashState);
            isAlreadyInAir = true;
        }
        
        // Speed up Fall
        if(player.input.Crouch && player.rb.velocity.y <= 0){
            player.rb.gravityScale = player.fastFallPower;
        }
            
    }

    

    public override void OnCollisionEnter2D(PlayerStateManager player, Collision2D collision){
        GameObject other = collision.gameObject;
        if(other.CompareTag("Enemy")) player.SwitchState(player.IdleState);
        if(other.CompareTag("ground")){
            player.robotJump = false;
            player.airBrake = 1f;
            player.speed = player.adjustSpeed;
            // reset gravity
            player.rb.gravityScale = player.GetOriginalGravity();
            player.air_dash_count = 0;
            // to Slide
            if(player.input.CrouchHeld && player.rb.velocity.x > 2f && player.crouch_ability){
                player.SwitchState(player.SlideState);
            }
            else if(player.input.SprintHeld && player.rb.velocity.x != 0 && player.sprint_ability){
                player.SwitchState(player.SprintState);
            }
            else if(player.input.CrouchHeld && player.crouch_ability){
                player.SwitchState(player.CrouchState);
            }
            else{
            // to Idle
            player.SwitchState(player.IdleState);
            }
            isAlreadyInAir = false;
        }

    }
}
