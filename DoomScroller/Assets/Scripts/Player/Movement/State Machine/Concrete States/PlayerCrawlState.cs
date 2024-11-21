using UnityEngine;

public class PlayerCrawlState : PlayerBaseState
{
    
    public override void EnterState(PlayerStateManager player){
        player.invul = false;
        if(GlobalControl.DebugEnabled) Debug.Log("Entering Crawl State");
        
        // Crouch Speed
        player.crouchSpeed = player.adjustCrouchSpeed;
        player.sprintSpeed = 1.0f;

      
        player.boxcollider2D.size = player.Crawl_V;
        player.boxcollider2D.offset = player.Default_Offset;
        if(player.Offset_Fix.localPosition.y > -2.37f){
            player.Offset_Fix.localPosition = new Vector3(player.Offset_Fix.localPosition.x,player.Offset_Fix.localPosition.y + player.Crouch_V_Offset.y,player.Offset_Fix.localPosition.z);
        }

        // Reset Rotation
        player.Rotcheck.localRotation = Quaternion.Euler(0,0,0);

        // Reset Momentum 
        player.timer = 0f;
        player.momentum = 0f;
    }

    public override void UpdateState(PlayerStateManager player){

        // to Walk
        if(!player.input.CrouchHeld){
            player.SwitchState(player.WalkState);
        }
        // to Crouch
        if(player.axis.x == 0){
            player.SwitchState(player.CrouchState);
        }
        // to Crouch Dash
        if(player.input.Dash && player.canDash && player.axis.y == 0 && player.dash_ability){
            player.SwitchState(player.CrouchDashState);
        }
        // to Dash
        if(player.input.Dash && player.canDash && player.axis.y != 0 && player.axis.x != 0 && player.dash_ability){
            //player.speed
            player.SwitchState(player.DashState);
        }
        // to Jump
        if(player.input.Jump && player.IsGrounded() && player.jump_ability){
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpingPower);
            // player.speed = 1f;
            player.sprintSpeed = 1f;
            player.animator.SetInteger("State", 10);
            player.SwitchState(player.JumpState);
        }
        //is Falling
        if(player.Falling() && !player.isDashing && !player.input.Dash && !player.input.DashHeld && !player.IsGrounded()){
            player.SwitchState(player.JumpState);
        }

        if(player.rb.velocity.x == 0f ){
            if(player.rb.velocity.y == 0f){
                player.timer += Time.deltaTime;
            }else{player.timer = 0f;}
        }else{player.timer = 0f;}
        
        if(player.timer > player.wait_time){
            player.timer = 0f;
            player.momentum = 0f;
            //Debug.Log("Reset Dash Multi Timer");
        }
        
        if(player.momentum >= 0f && player.rb.velocity.x >= 0f && player.axis.x <= 0){
            player.momentum = 0f;
        }
        
        if(player.momentum >= 0f && player.rb.velocity.x <= 0f && player.axis.x >= 0){
            player.momentum = 0f;
        }
        
    }

    public override void OnCollisionEnter2D(PlayerStateManager player, Collision2D collision){

    }
}
