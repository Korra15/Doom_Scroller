using UnityEngine;

public class PlayerSprintState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player){
        player.invul = false;
        if(GlobalControl.DebugEnabled) Debug.Log("Entering Sprint State");
        // Sprint Speed
        player.sprintSpeed = player.adjustSprintSpeed;

        // Animations | Box Collider | Offsets
        // player.animator.SetInteger("Attack", 0);
        player.boxcollider2D.size = player.IJJSAC_V;
        player.boxcollider2D.offset = player.Default_Offset;

        // Sound

        // Reset Rotation
        player.Rotcheck.localRotation = Quaternion.Euler(0,0,0);
    }

    public override void UpdateState(PlayerStateManager player){
        
         // to Idle
        if ((player.input.SprintReleased && player.rb.velocity.x == 0))
        {
            player.SwitchState(player.IdleState);
        }
        // to Walk
        if (player.input.SprintReleased)
        {
            //PlayerAudio.instance.StopSFXLoop(PlayerAudio.instance.jogging); // stopping jogging happens in PlayerWalkState.cs
            player.SwitchState(player.WalkState);
        }
        // to Slide
        if(player.input.Crouch && player.crouch_ability){
            player.SwitchState(player.SlideState);
        }
        // to Idle
        if(player.axis.x == 0){
            player.SwitchState(player.IdleState);
        }
        // to Jump
        if(player.input.JumpHeld && player.IsGrounded() && player.jump_ability){
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpingPower);
            player.SwitchState(player.JumpState);
        }
        if(player.Falling()){
            player.SwitchState(player.JumpState);
        }
        //to Dash
        if(player.input.Dash && player.canDash && player.dash_ability){
            player.SwitchState(player.DashState);
        }
    }

    public override void OnCollisionEnter2D(PlayerStateManager player, Collision2D collision){

    }
}
