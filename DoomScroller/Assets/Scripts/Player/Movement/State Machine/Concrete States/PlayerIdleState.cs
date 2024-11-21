using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player){
        player.invul = false;

//        if(GlobalControl.DebugEnabled) Debug.Log("Entering Idle State");

         // Animations | Box Collider | Offsets
       
        player.boxcollider2D.size = player.IJJSAC_V;
        player.boxcollider2D.offset = player.Default_Offset;

        // Reset Rotation
        player.Rotcheck.localRotation = Quaternion.Euler(0,0,0);
    }

    public override void UpdateState(PlayerStateManager player){

        // Time Still
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
        
        // to Walk
        // if(player.horizontal != 0){
        if(player.axis.x != 0){
            player.SwitchState(player.WalkState);
        }
        // to Crouch
        if(player.input.Crouch && player.crouch_ability){
            player.SwitchState(player.CrouchState);
        }
        if((player.input.JumpHeld || player.input.Jump) && player.IsGrounded() && player.jump_ability){
            // if(GlobalControl.DebugEnabled) //Debug.Log("Jumping");
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpingPower);
            player.SwitchState(player.JumpState);
        }
        // if(player.Falling()){
        if(player.Falling() && !player.IsGrounded()){
            player.SwitchState(player.JumpState);
        }
        // to Dash
        if(player.input.Dash && player.canDash && player.dash_ability)
        {
            player.SwitchState(player.DashState);
        }
    }

    public override void OnCollisionEnter2D(PlayerStateManager player, Collision2D collision){



        }
    }
