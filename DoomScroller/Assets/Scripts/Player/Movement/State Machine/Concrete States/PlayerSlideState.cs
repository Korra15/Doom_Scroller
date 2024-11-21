using System.Collections;
using UnityEngine;

public class PlayerSlideState : PlayerBaseState
{
    float originalGravity;

    public override void EnterState(PlayerStateManager player){
        player.invul = false;

        player.slide_timer = 0;

        // Animations | Box Collider | Offsets

        player.boxcollider2D.size = player.Slide_V;
        player.boxcollider2D.offset = player.Default_Offset;
        if(player.Offset_Fix.localPosition.y > -2.37f){
            player.Offset_Fix.localPosition = new Vector3(player.Offset_Fix.localPosition.x,player.Offset_Fix.localPosition.y + player.Crouch_V_Offset.y,player.Offset_Fix.localPosition.z);
        }

        // Reset rotation
        player.Rotcheck.localRotation = Quaternion.Euler(0,0,0);
    }

    public override void UpdateState(PlayerStateManager player){
        
        // to Jump
        if(player.input.Jump && player.IsGrounded() && player.jump_ability){
            player.SwitchState(player.JumpState);
        }
        if(player.Falling()){
            player.SwitchState(player.JumpState);
        }
        
        // to Crouch Dash
        if(player.input.Dash && player.canDash && (player.axis.y > 0) && (player.axis.x != 0) && player.dash_ability){
            player.SwitchState(player.DashState);
        }

        if(player.input.Dash && player.canDash && player.dash_ability){
            player.SwitchState(player.CrouchDashState);
        }
        if(player.input.Crouch && player.crouch_ability){
            // to Idle
            if(player.axis.x == 0){
                player.SwitchState(player.IdleState);
            }
            // to Sprint
            else if (player.input.SprintHeld && player.rb.velocity.x != 0 && player.sprint_ability)
            {
                player.SwitchState(player.SprintState);
            }
            // to Walk
            else
            {
                player.SwitchState(player.WalkState);
            }
        }
        // to Crawl
        if(Mathf.Abs(player.rb.velocity.x) <= player.speed/2 && player.axis.x != 0){
            player.SwitchState(player.CrawlState);
        }
        // to Crouch
        if(player.rb.velocity.x >= 0 && player.axis.x <= 0){
            player.SwitchState(player.CrouchState);
        }
        if(player.rb.velocity.x <= 0 && player.axis.x >= 0){
            player.SwitchState(player.CrouchState);
        }
    }

    public override void OnCollisionEnter2D(PlayerStateManager player, Collision2D collision){

    }
}
