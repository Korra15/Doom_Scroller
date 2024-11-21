using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    bool dashFinish;
    float direction;

    float temp_drag;
    public override void EnterState(PlayerStateManager player)
    {
        dashFinish = false;
        player.invul = false;

        // Animations | Box Collider | Offsets

        PlayerAudio.instance.PlayOneShot(PlayerAudio.instance.dash);//0.35f volume
        player.boxcollider2D.size = player.CrouchD_V;
        player.boxcollider2D.offset = player.Dash_Offset;
        if(player.Offset_Fix.localPosition.y > -2.37f){
            player.Offset_Fix.localPosition = new Vector3(player.Offset_Fix.localPosition.x,player.Offset_Fix.localPosition.y + player.Crouch_V_Offset.y,player.Offset_Fix.localPosition.z);
        }
        
        // Direction of Dash
        if (player.isFacingRight) direction = 1f;
        else if (!player.isFacingRight) direction = -1f;

        // Check for Air Dash Amount
        if(player.air_dash_count < player.air_dash_max){
            // Invincible Dash | Normal Dash
            if (player.invis_dash_unlock == true)
            {
                player.invul = true;
                if(GlobalControl.DebugEnabled) Debug.Log("Entering Invincible Dash State");
                player.StartCoroutine(InvincibleDash(player));
            }
            else
            {
                player.invul = false;
                if(GlobalControl.DebugEnabled) Debug.Log("Entering Normal Dash State");
                player.StartCoroutine(Dash(player));
            } 

            // Reset Momentum
            if(player.momentum >= player.momentum_cap){
                player.momentum = player.momentum_cap;
            }

            if (player.isFacingRight)
            {
                if (player.isDiagonal)
                {
                    if (player.rb.velocity.y > 0 && player.rb.velocity.x != 0)
                    {
                        player.Rotcheck.localRotation = Quaternion.Euler(0, 0, 45f);
                    }
                    else if (player.rb.velocity.y < 0 && player.rb.velocity.x != 0)
                    {
                        player.Rotcheck.localRotation = Quaternion.Euler(0, 0, -45f);
                    }
                }
                else if (player.axis.y == 1)
                {
                    player.Rotcheck.localRotation = Quaternion.Euler(0, 0, 90);
                }
                else if (player.axis.y == -1)
                {
                    player.Rotcheck.localRotation = Quaternion.Euler(0, 0, -90);
                }
            }
            else if (!player.isFacingRight)
            {
                if (player.isDiagonal)
                {
                    if (player.rb.velocity.y > 0 && player.rb.velocity.x != 0)
                    {
                        player.Rotcheck.localRotation = Quaternion.Euler(0, 0, -45f);
                    }
                    else if (player.rb.velocity.y < 0 && player.rb.velocity.x != 0)
                    {
                        player.Rotcheck.localRotation = Quaternion.Euler(0, 0, 45f);
                    }
                }
                else if (player.axis.y == 1)
                {
                    player.Rotcheck.localRotation = Quaternion.Euler(0, 0, -90);
                }
                else if (player.axis.y == -1)
                { 
                    player.Rotcheck.localRotation = Quaternion.Euler(0, 0, 90);
                }

            }
        }else{
            dashFinish = true;
        }

        if(!player.IsGrounded()){
            player.air_dash_count++;
        }
    }

    public override void UpdateState(PlayerStateManager player)
    {
        //Switch to Slide State
        if (player.input.Crouch && player.Falling() == false && player.crouch_ability)
        {
            player.Rotcheck.localRotation = Quaternion.Euler(0,0,0);
            player.SwitchState(player.SlideState);
        }
        
        if(dashFinish){

            // var emission = player.smoke.emission;
            // emission.rateOverTime = 0; 

            player.Rotcheck.localRotation = Quaternion.Euler(0,0,0);

            //Switch to Idle State
            if (player.axis.x == 0 && player.IsGrounded()) 
            {
                player.Rotcheck.localRotation = Quaternion.Euler(0,0,0);
                player.SwitchState(player.IdleState);
            }
            //Switch to Walk State
            if (player.input.DashReleased && player.IsGrounded() && player.axis.x != 0)
            {
                player.Rotcheck.localRotation = Quaternion.Euler(0,0,0);
                player.SwitchState(player.WalkState);
            }
            //Switch to Sprint State
            if (player.input.SprintHeld && player.rb.velocity.x != 0 && player.sprint_ability)
            {
                player.Rotcheck.localRotation = Quaternion.Euler(0,0,0);
                player.SwitchState(player.SprintState);
            }
            //Switch to Slide State
            if (player.input.Crouch && player.Falling() == false && player.crouch_ability)
            {
                player.Rotcheck.localRotation = Quaternion.Euler(0,0,0);
                player.SwitchState(player.SlideState);
            }
            //is falling
            if (!player.IsGrounded()){
                player.Rotcheck.localRotation = Quaternion.Euler(0,0,0);
                //player.SwitchState(player.JumpState); <- Changed from JumpState to WalkState to allow dashes to break air control
                player.SwitchState(player.WalkState);
            }
            if (player.IsGrounded() && player.axis.x != 0)
            {
                //Debug.Log(player.axis.x);
                player.SwitchState(player.WalkState);
            }
        }

    }

    public override void OnCollisionEnter2D(PlayerStateManager player,Collision2D collision)
    {

    }

    public IEnumerator InvincibleDash(PlayerStateManager player)
    {
        player.canDash = false;
        Physics2D.IgnoreLayerCollision(8,6,true);
        temp_drag = player.rb.drag;
        player.rb.drag = 0f;
        float originalGravity = player.rb.gravityScale;
        player.rb.gravityScale = 0f;

        if ((player.axis.y > 0 || player.axis.y < 0) && (player.axis.x == 0)){
            //vertical dash
            // if(GlobalControl.DebugEnabled) Debug.Log("1");
            player.rb.velocity = new Vector2(0, player.axis.y * player.dashingPower * 0.7f * Mathf.Sqrt(2f)); // Double dash power to make it match the same distance as a normal dash
        } else if(player.axis.y == -1 && player.IsGrounded() && player.axis.x != 0){
            //dash on ground
            // if(GlobalControl.DebugEnabled) Debug.Log("2");
            player.rb.velocity = new Vector2(player.axis.x * player.dashingPower * 0.7f * Mathf.Sqrt(2f), 0);
        } else{
            // if(GlobalControl.DebugEnabled) Debug.Log("3");
            player.rb.velocity = new Vector2(direction * player.dashingPower, player.axis.y * player.dashingPower * 0.7f);
        }
        player.momentum += player.momentum_inc;

        yield return new WaitForSeconds(player.dashingTime);
        if (player.isDiagonal && (!player.IsGrounded() && player.axis.y != -1)) player.rb.velocity = new Vector2(direction * player.speed, player.dashingPower / 3);
        else player.rb.velocity = new Vector2(direction * player.speed * player.sprintSpeed, 0);

        //need to check if player is dashing diagonally downward
        //need to check if player is dashing diagonaly
        player.rb.gravityScale = originalGravity;
        dashFinish = true;
        player.isCrowDashing = false;

        Physics2D.IgnoreLayerCollision(8,6,false);
        yield return new WaitForSeconds(player.dashingCooldown);
        player.canDash = true;
        player.rb.drag = temp_drag;
        player.rb.angularDrag = 0.05f;
    }

    public IEnumerator Dash(PlayerStateManager player)
    {
        player.canDash = false;
        float originalGravity = player.rb.gravityScale;
        player.rb.gravityScale = 0f;
        temp_drag = player.rb.drag;
        player.rb.drag = 0f;
        player.rb.angularDrag = 0.05f;

        // var emission = player.smoke.emission;
        // emission.rateOverTime = 75;
        if ((player.axis.y > 0 || player.axis.y < 0) && (player.axis.x == 0)){
            // if(GlobalControl.DebugEnabled) Debug.Log("1");
            player.rb.velocity = new Vector2(0, player.axis.y * player.dashingPower * 0.7f * Mathf.Sqrt(2)); // Double dash power to make it match the same distance as a normal dash
        } else{
            // if(GlobalControl.DebugEnabled) Debug.Log("3");
            player.rb.velocity = new Vector2(direction * player.dashingPower * Mathf.Sqrt(2f), player.axis.y * player.dashingPower * 0.7f);
        }
        player.momentum += player.momentum_inc;

        yield return new WaitForSeconds(player.dashingTime);
        if (player.isDiagonal || (player.rb.velocity.y > 0)) player.rb.velocity = new Vector2(direction * player.speed, player.dashingPower / 3);
        else player.rb.velocity = new Vector2(direction * player.speed * player.sprintSpeed, 0);
        player.rb.gravityScale = originalGravity;
        dashFinish = true;
        player.isCrowDashing = false;

        yield return new WaitForSeconds(player.dashingCooldown);
        player.canDash = true;
        player.rb.drag = temp_drag;
    }
}
