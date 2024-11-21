using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyPlatform : MonoBehaviour
{

    public GameObject Player;
    // public GameObject Turtle;

    private PlayerStateManager stateManagerScript;
    public int bounceFactor;
    public int heightLimit;
    private float yVal;
    public int countdown;
    public Renderer rend;
    public Collider2D collide;

    private void Start()
    {
        stateManagerScript = Player.GetComponent<PlayerStateManager>();
        bounceFactor = 20;
        countdown = 3;
        heightLimit = 10;
        rend = GetComponent<Renderer>();
        collide = GetComponent<Collider2D>();
        // yVal = Turtle.transform.position.y;


    }

    void Update()
    {
        yVal += 0.05f;
        // transform.position = new Vector2(Turtle.transform.position.x, yVal);

        if (yVal > heightLimit)
        {
            // yVal = Turtle.transform.position.y;
        }
    }

        private IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(countdown);
            rend.enabled = true;
            collide.enabled = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("On bounce platform");
            //collision.gameObject.GetComponent<Rigidbody2D>().drag = 0;
            stateManagerScript.onBounceSurface = true;
            Debug.Log(collision.gameObject.isStatic);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounceFactor, ForceMode2D.Impulse);
            //collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(20,20);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            stateManagerScript.onBounceSurface = false;
        }
        rend.enabled = false;
        collide.enabled = false;
        StartCoroutine(Timer());


    }


}
