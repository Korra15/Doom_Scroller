using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public LockAndKey lockAndKey;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.getSfXSource(),AudioManager.instance.KeyPickup);
        }
   
        if (lockAndKey != null && collision.gameObject.CompareTag("Player") ){ 
            lockAndKey.SetOpen();
            Destroy(this.gameObject);
        }
    }
}
