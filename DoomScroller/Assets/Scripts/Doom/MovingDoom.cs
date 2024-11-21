using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingDoom : MonoBehaviour
{
    public float speed;
    public float idealSpeed = 5f;
    public GameObject endPoint;
    public Chunk currentChunk;
  

    private void Start()
    {
        speed = idealSpeed;
    }
    void Update()
    {
       // Debug.Log(PersistentGameData.score);    
        // Calculate movement on the X axis at a constant pace
        Vector3 movement = new Vector3(speed * Time.deltaTime, 0f, 0f);

        // Apply movement to the GameObject
        transform.Translate(movement);

        if (endPoint != null && endPoint.transform.position.x < transform.position.x)
        {
            currentChunk.manager.RemoveFirstChunk();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Chunk>() && currentChunk == null)
        {
            endPoint = currentChunk.endPoint;
        }
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("dead");
            
            //SceneManager.LoadScene(0);
            SceneManager.LoadScene(4);

        }
    }
}
