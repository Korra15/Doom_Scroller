using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDoom : MonoBehaviour
{
    public MovingDoom impendingDoom;
    public float slowerSpeed = 2f;
    public float slowDoomDuration;

    private void OnTriggerEnter2D(Collider2D other)
    {
            if (other.gameObject.tag == "Player")
            {
                StartCoroutine(SlowDoomForDuration(slowDoomDuration));
            }
    }

    private void ApplySlowDoomSpeed(float slowerSpeed)
    {
        impendingDoom.speed = slowerSpeed;
        Debug.Log("Slow Doom");
    }

    private void DisableSlowDoomSpeed(float idealDoomSpeed)
    {
        impendingDoom.speed = idealDoomSpeed;
        Debug.Log("Reset Doom");
    }
    IEnumerator SlowDoomForDuration(float slowDoomDuration)
    {
        ApplySlowDoomSpeed(slowerSpeed);
        Destroy(gameObject.GetComponent<SpriteRenderer>());    
        yield return new WaitForSeconds(slowDoomDuration);
       
        Debug.Log("Action complete after " + slowDoomDuration + " seconds.");
        DisableSlowDoomSpeed(impendingDoom.idealSpeed);
        Destroy(gameObject);
    }

}
