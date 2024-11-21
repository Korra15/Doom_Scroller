using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMenu : MonoBehaviour
{

    public PlayerStateManager player; // Variable for PSM

    public GameObject SprintObject;
    //public GameObject MoveObject;
    public GameObject JumpObject;
    public GameObject DashObject;
    public GameObject CrouchObject;
    public GameObject BlockObject;
    public GameObject AttackObject;
    //public GameObject HudObject;
    //public GameObject InteractObject;
    public GameObject GlideObject;

    void Start(){
        if(player == null){
            player = GameObject.Find("Player").GetComponent<PlayerStateManager>(); // If PSM not assigned try and find.
        }
    }

    void Update(){
        SprintObject.SetActive(player.sprint_ability);
        //MoveObject.SetActive(true);
        JumpObject.SetActive(player.jump_ability);
        DashObject.SetActive(player.dash_ability);
        CrouchObject.SetActive(player.crouch_ability);
    }
}
