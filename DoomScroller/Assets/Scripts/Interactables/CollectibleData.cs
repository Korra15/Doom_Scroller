using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleData : MonoBehaviour
{
    public bool jumpUnlocked;

    public CollectibleData(CollectibleManager collectible){
        jumpUnlocked = collectible.jumpUnlocked;
    }
}
