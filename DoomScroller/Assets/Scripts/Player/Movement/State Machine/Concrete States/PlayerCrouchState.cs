using UnityEngine;

public class PlayerCrouchState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player){
        if(GlobalControl.DebugEnabled) Debug.Log("Entering Crouch State");
        player.invul = false;
        
        // Reset movement
        player.sprintSpeed = 1.0f;

        // Animations | Box Collider | Offsets

        player.boxcollider2D.size = player.Crouch_V;
        player.boxcollider2D.offset = player.Default_Offset;
        if(player.Offset_Fix.localPosition.y > -2.37f){
            player.Offset_Fix.localPosition = new Vector3(player.Offset_Fix.localPosition.x,player.Offset_Fix.localPosition.y + player.Crouch_V_Offset.y,player.Offset_Fix.localPosition.z);
        }

        // Reset Momentum
        player.timer = 0f;
        player.momentum = 0f;
    }

    public override void UpdateState(PlayerStateManager player){


        //player.animator.SetInteger("State", 2);
        // to Idle
        if (!player.input.CrouchHeld){
            player.SwitchState(player.IdleState);
        }

        //to Jump
        if((player.input.Jump) && player.IsGrounded() && player.jump_ability){
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpingPower);
            PlayerAudio.instance.PlayOneShot(PlayerAudio.instance.crouchJump);//0.5f volume
            player.SwitchState(player.JumpState);
        }
        if(player.Falling()){
            player.SwitchState(player.JumpState);
        }
        // to Crawl
        if(player.axis.x != 0){
            player.SwitchState(player.CrawlState);
        }
        // to Dash
        if(player.input.Dash && player.canDash && player.axis.y == 1 && player.dash_ability){
            player.SwitchState(player.DashState);
        }
        if(player.input.Dash && player.canDash && player.axis.x == 0 && player.dash_ability){
            player.SwitchState(player.CrouchDashState);
        }
        // to Crouch Dash
        if(player.input.Dash && player.canDash && (player.axis.x != 0) && player.axis.y != 1 && player.dash_ability){
            player.SwitchState(player.CrouchDashState);
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
