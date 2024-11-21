using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public bool jumpUnlocked = false;
    public SaveManager save;

    void Start()
    {
        LoadCollectible();
    }

    public void LoadCollectible()
    {
        GameData data = save.LoadPlayerData();
        jumpUnlocked = data.jump_ability;
    }
}
