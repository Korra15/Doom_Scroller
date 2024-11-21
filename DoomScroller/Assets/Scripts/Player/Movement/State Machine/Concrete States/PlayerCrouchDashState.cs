using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchDashState : PlayerBaseState
{
    bool dashFinish, verifyBoost;
    float direction;
    public override void EnterState(PlayerStateManager player)
    {
        player.invul = false;
        dashFinish = false;

        var smokeEmission = player.smoke.emission;
        smokeEmission.rateOverTime = 75;

        // Animations | Box Collider | Offsets
    
        player.boxcollider2D.size = player.CrouchD_V;
        player.boxcollider2D.offset = player.CrouchD_Offset;
        if(player.Offset_Fix.localPosition.y > -2.37f){
            player.Offset_Fix.localPosition = new Vector3(player.Offset_Fix.localPosition.x,player.Offset_Fix.localPosition.y + player.Crouch_V_Offset.y,player.Offset_Fix.localPosition.z);
        }

        // Direction of Dash
        if(player.isFacingRight){
            direction = 1f;
        }else{
            direction = -1f;
        }
 
        // Invincible Dash | Normal Dash
        if (player.invis_dash_unlock == true)
        {
            player.invul = true;
            if(GlobalControl.DebugEnabled) Debug.Log("Entering Invincible CrouchDashState I");
            player.StartCoroutine(InvincibleDash(player));
        }
        else
        {
            if(GlobalControl.DebugEnabled) Debug.Log("Entering CrouchDashState");
            player.StartCoroutine(Dash(player));
        }

        // Reset Momentum
        if(player.momentum >= player.momentum_cap){
            player.momentum = player.momentum_cap;
        }
    }

    public override void UpdateState(PlayerStateManager player)
    {
        
        if(dashFinish){
            var smokeEmission = player.smoke.emission;
            smokeEmission.rateOverTime = 0;
            //Switch to Idle State
            if(player.axis.x == 0)
            {
                player.SwitchState(player.CrouchState);
            }
            //Switch to Crawl State
            if (player.input.SprintReleased)
            {
                player.SwitchState(player.CrawlState);
            }
            //Switch to Sprint State
            if (player.input.SprintHeld && player.rb.velocity.x != 0 && player.sprint_ability)
            {
                player.SwitchState(player.SprintState);
            }
            //Switch to Crouch State
            if(player.input.CrouchHeld)
            {
                player.SwitchState(player.CrouchState);
            }
            // to Jump
            if(player.input.Jump && player.IsGrounded()){
                player.momentum = player.momentum_cap;
                player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpingPower);
                player.SwitchState(player.JumpState);
            }
            if(player.Falling()){
                player.SwitchState(player.JumpState);
            }
            if (verifyBoost) {
                player.momentum = player.momentum_cap;
                player.SwitchState(player.JumpState); //idk if i need this
                verifyBoost = false; //idk if i need this either
            }

            // if(Input.GetKey(KeyCode.Space))
            // {
            //     player.momentum = player.momentum_cap;
            //     player.SwitchState(player.JumpState);
            // }
            
            if(player.input.DashHeld && player.canDash && player.dash_ability){
                player.SwitchState(player.DashState);
            }

            // - - - - - This line of code triggers the infinite crawl-dash-jump
            //player.SwitchState(player.CrawlState);
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            if(!player.input.CrouchHeld) {
                player.SwitchState(player.WalkState);
            }
        }

        if(!dashFinish){
            if (player.isJumping) {
                verifyBoost = true;
                player.momentum = player.momentum_cap;
            }
            if(player.isFacingRight){
                if(player.isDiagonal){
                    if(player.rb.velocity.y > 0 && player.rb.velocity.x != 0){
                        player.Rotcheck.localRotation = Quaternion.Euler(0,0,45f);
                    }else if(player.rb.velocity.y < 0 && player.rb.velocity.x != 0){
                        player.Rotcheck.localRotation = Quaternion.Euler(0,0,-45f);
                    }
                }else if (player.axis.y == 1){
                    player.Rotcheck.localRotation = Quaternion.Euler(0,0,90);
                }else if(player.axis.y == -1){
                    player.Rotcheck.localRotation = Quaternion.Euler(0,0,-90);
                }
            }else{
                if(player.isDiagonal){
                    if(player.rb.velocity.y > 0 && player.rb.velocity.x != 0){
                        player.Rotcheck.localRotation = Quaternion.Euler(0,0,-45f);
                    }else if(player.rb.velocity.y < 0 && player.rb.velocity.x != 0){
                        player.Rotcheck.localRotation = Quaternion.Euler(0,0,45f);
                    }
                }else if (player.axis.y == 1){
                    player.Rotcheck.localRotation = Quaternion.Euler(0,0,-90);
                }else if(player.axis.y == -1){
                    player.Rotcheck.localRotation = Quaternion.Euler(0,0,90);
                }

            }
        }
    }

    public override void OnCollisionEnter2D(PlayerStateManager player,Collision2D collision)
    {
    
    }
    
    private IEnumerator InvincibleDash(PlayerStateManager player)
    {
        player.canDash = false;
        Physics2D.IgnoreLayerCollision(8,6,true);

        float originalGravity = player.rb.gravityScale;
        player.rb.gravityScale = 0f;

        if ((player.axis.y > 0 || player.axis.y < 0) && (player.axis.x == 0)){
            player.rb.velocity = new Vector2(0, player.axis.y * player.dashingPower * 0.7f);
        } else{
            player.rb.velocity = new Vector2(player.dashingPower * direction * 0.7f, 0);
        }
        player.momentum += player.momentum_inc;
 
        yield return new WaitForSeconds(player.dashingTime);
        player.rb.gravityScale = originalGravity;
        dashFinish = true;

        Physics2D.IgnoreLayerCollision(8,6,false);
        yield return new WaitForSeconds(player.dashingCooldown);
        player.canDash = true;
    }
    private IEnumerator Dash(PlayerStateManager player)
    {
        player.canDash = false;
        float originalGravity = player.rb.gravityScale;
        player.rb.gravityScale = 0f;
        
        if ((player.axis.y > 0 || player.axis.y < 0) && (player.axis.x == 0)){
            player.rb.velocity = new Vector2(0, player.axis.y * player.dashingPower * 0.7f);
        } else{
            player.rb.velocity = new Vector2(player.dashingPower * direction * 0.7f, 0);
        }
        player.momentum += player.momentum_inc;

        yield return new WaitForSeconds(player.dashingTime);
        player.rb.gravityScale = originalGravity;
        dashFinish = true;

        yield return new WaitForSeconds(player.dashingCooldown);
        player.canDash = true;
    }
}