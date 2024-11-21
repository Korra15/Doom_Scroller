using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using TMPro;


public class Unlockable : MonoBehaviour
{
    public PlayerStateManager player;
    public SaveManager save;
    public UserInput input;
    public int ability_id;
    public enum UnlockableType {ability} 
    [SerializeField] UnlockableType unlockableType;

    public bool canPickUp = false;
    private float t = 0;
    public TextMeshProUGUI rideText;
    public Color newAlpha;
    public float fadeTime = 0.5f;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player").GetComponent<PlayerStateManager>();
        }
        if (save == null)
        {
            save = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        }
        if (input == null)
        {
            input = GameObject.Find("InputManager").GetComponent<UserInput>();
        }
    }

    void Update()
    {
        if (unlockableType == UnlockableType.ability)
            AbilityDestroy(ability_id);

        if (canPickUp && input.Interact)
            PickUp();
    }

    void FixedUpdate()
    {
        if (canPickUp)
            newAlpha.a = 1f;  // Assuming this is meant for full opacity in Color
        else
            newAlpha.a = 0f;  // Assuming this is meant for no opacity in Color
        TextFade();
    }

    void AbilityUnlock(int id){
        switch (id){
            case 1:
                player.jump_ability = true;
                break;
            case 2:
                player.dash_ability = true;
                break;
            case 3:
                player.invis_dash_unlock = true;
                break;
            case 4:
                player.crouch_ability = true;
                break;
            case 5:
                player.sprint_ability = true;
                break;
        }
    }

    void AbilityDestroy(int id){
        switch (id){
            case 1:
                if (player.jump_ability) Destroy(gameObject);
                break;
            case 2:
                if (player.dash_ability) Destroy(gameObject);
                break;
            case 3:
                if (player.invis_dash_unlock) Destroy(gameObject);
                break;
            case 4:
                if (player.crouch_ability) Destroy(gameObject);
                break;
            case 5:
                if (player.sprint_ability) Destroy(gameObject);
                break;
        }
    }

    public void PickUp()
    {
        if (unlockableType == UnlockableType.ability)
        {
            AbilityUnlock(ability_id);
            AbilityDestroy(ability_id);
        }
        save.Save();
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player") 
        {
            canPickUp = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            canPickUp = false;
    }

    void TextFade()  // Make the text fade in or out
    {
        t = Mathf.Pow(0.5f, Time.deltaTime * fadeTime);
        rideText.color = Color.Lerp(rideText.color, new Color(newAlpha.r, newAlpha.g, newAlpha.b, newAlpha.a), t);
    }
}
