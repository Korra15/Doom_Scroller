using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawBone : MonoBehaviour
{
    public Rigidbody2D connectedLink; // The previous link or base that this link is connected to
    public float maxDistance = 0.5f;  // The maximum allowed distance between links
    public float reconnectForce = 100f; // Force applied to reconnect if separation occurs
    

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void FixedUpdate()
    {
        // Monitor the distance between this link and the connected one
        if (connectedLink != null)
        {
            float currentDistance = Vector2.Distance(transform.position, connectedLink.position);

            if (currentDistance > maxDistance)
            {
                // Apply a force to pull the chain link back if it exceeds the max distance
                Vector2 direction = (connectedLink.position - (Vector2)transform.position).normalized;
                rb.AddForce(direction * (currentDistance - maxDistance) * reconnectForce);
            }
        }
    }
}
