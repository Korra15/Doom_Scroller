using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // These are the data you want to save about the player
    public int current_health;
    public bool jump_ability;
    public bool dash_ability;
    public bool invis_dash_unlock;
    public bool crouch_ability;
    public bool sprint_ability;
    public double pickups;
    public int maxHealth;

    public bool opening_cutscene_played;
    public bool spawn_gate;


    // This is a constructor that consturcts a PlayerData object with the fields above
    // If you add something to the fields above you have to add it here.
    public GameData(PlayerStateManager Player){
        if (Player != null)
        {
            this.current_health = Player.current_health;
            this.pickups = Player.pickups;

            this.jump_ability = Player.jump_ability;
            this.dash_ability = Player.dash_ability;
            this.invis_dash_unlock = Player.invis_dash_unlock;
            this.crouch_ability = Player.crouch_ability;
            this.sprint_ability = Player.sprint_ability;

            this.opening_cutscene_played = Player.opening_cutscene_played;
            this.spawn_gate = Player.spawn_gate;
        }
    }


    // Default Values 
    public GameData(){
        this.current_health = 20;
        this.pickups = 0;

        this.jump_ability = false;
        this.dash_ability = false;
        this.invis_dash_unlock = false;
        this.crouch_ability = false;
        this.sprint_ability = false;

        this.opening_cutscene_played = false;
        this.spawn_gate = false;
    }
}
