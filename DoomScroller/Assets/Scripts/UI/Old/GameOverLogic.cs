using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverLogic : MonoBehaviour
{
    public GameObject Player;
    public Entity PlayerHealth;
    public GameObject gameOverUI;

    //game over stuff
    public PlayerStateManager playerStateManager;
    private bool healedMax = false;

    // Start is called before the first frame update
    void Start()
    {
        healedMax = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Game over logic. Disables player then respawns. Currently uses FadeOutAndLoadScene() in SceneChangeManager.cs as a transition animation. 
        // if (PlayerHealth.CurrentHealth <=0)
        // {
        //     Player.SetActive(false);
        //     gameOverUI.SetActive(true);
        //     Respawn();
        // }
    }

    public void Respawn()
    {
        // playerStateManager.CurrentHealth = playerStateManager.MaxHealth;
        // PlayerHealth.health = PlayerHealth.maxHealth;
        playerStateManager.save.Save();
    }
}
