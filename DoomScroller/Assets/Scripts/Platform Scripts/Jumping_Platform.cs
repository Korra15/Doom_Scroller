using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping_Platform : MonoBehaviour        
{
    public float jumpForce;
    public bool effectAllObjects = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(effectAllObjects)
        {
            if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up  * jumpForce, ForceMode2D.Impulse);
            }
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }


}
