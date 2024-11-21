using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    //PlayerAudio audio;

    public override void EnterState(PlayerStateManager player){
        player.invul = false;
    //    if(GlobalControl.DebugEnabled) Debug.Log("Entering Walk State");
        // Reset Speed
        player.sprintSpeed = 1;
        player.crouchSpeed = 1;
        player.speed = player.adjustSpeed;

        // Animations | Box Collider | Offsets
       
        player.boxcollider2D.size = player.IJJSAC_V;
        player.boxcollider2D.offset = player.Default_Offset;

        // Sound

        // Reset Rotation
        player.Rotcheck.localRotation = Quaternion.Euler(0,0,0);
    }

    public override void UpdateState(PlayerStateManager player){

        if(player.momentum >= 0f && player.rb.velocity.x >= 0f && player.axis.x <= 0){
            player.momentum = 0f;
        }
        
        if(player.momentum >= 0f && player.rb.velocity.x <= 0f && player.axis.x >= 0){
            player.momentum = 0f;
        }
        // to Idle
        if(player.axis.x == 0){
            player.SwitchState(player.IdleState);
        }
        // to Sprint
        if (player.input.SprintHeld && player.rb.velocity.x != 0)
        {
            //PlayerAudio.instance.StopSFXLoop(PlayerAudio.instance.footsteps); // stopping footsteps happens in PlayerSprintState.cs
            player.SwitchState(player.SprintState);
        }
        // to Crawl
        if(player.input.Crouch && player.crouch_ability){
            player.SwitchState(player.CrawlState);
        }
        // to Jump
        if(player.input.JumpHeld && player.IsGrounded() && player.jump_ability){
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpingPower);
            player.SwitchState(player.JumpState);
        }
        if(player.Falling())
        {
            player.SwitchState(player.JumpState);
        }
        // to dash
        if(player.input.Dash && player.canDash && player.dash_ability){
            player.SwitchState(player.DashState);
        }
    }
    
    public override void OnCollisionEnter2D(PlayerStateManager player, Collision2D collision){

    }
}
