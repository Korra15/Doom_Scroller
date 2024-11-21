using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakble : MonoBehaviour
{

    public GameObject [] childern;
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
        if (collision.transform.GetChild(0).gameObject.GetComponent<HeavyPlatform>() != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.getSfXSource(),AudioManager.instance.BreakablePlatfrom);
            foreach (GameObject child in childern)
            {
                child.AddComponent<Rigidbody2D>();
                child.transform.SetParent(null);
                Destroy(this.GetComponent<BoxCollider2D>());
            }
            Destroy(this);
        }
    }
   
}
