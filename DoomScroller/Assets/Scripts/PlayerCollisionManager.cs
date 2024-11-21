using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisionManager : MonoBehaviour
{
    public float minHighet;
    public Chunk currentchunk = null;
    float yToLose;
    bool inAir = true;
    Rigidbody2D rigidbody;
    public Animator animator;
    public Explosion explosion;
    public PlayerStateManager state;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        yToLose = minHighet;
    }
    public void Update()
    {        
        //Check if in air
        if(rigidbody.velocity.magnitude > 0)
        {
            if(!animator.GetBool("IsMoving"))
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }


        if(state.IsGrounded())
        {
            animator.SetBool("InAir", false);
        }
        else
        {
            animator.SetBool("InAir", true);
            animator.SetBool("IsMoving", false);
        }


        if (gameObject.transform.position.y < yToLose)
        {
            GameOver();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
     
         if (collision.gameObject.tag == "Ground")
        {
            animator.SetBool("inAir", false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ChunkExit")
        {
            if (  currentchunk.firstFinish == true )
            {
                currentchunk.firstFinish = false;
                currentchunk = currentchunk.manager.SpawnRandomChunk();
                yToLose = currentchunk.transform.position.y + minHighet;
                Debug.Log("new chunk! = " + currentchunk.gameObject.transform.position);
            }
        }
        else if (collision.gameObject.tag == "Ground")
        {
            animator.SetBool("inAir", true);

        }
    }
    public void GotHit( int damage)
    {
        GameOver();
    }
    public void GameOver()
    {
        explosion.Explode(this.gameObject);
  

    }

}
