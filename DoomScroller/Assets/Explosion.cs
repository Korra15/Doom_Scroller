using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject[] botPieces;
    public float radius = 5.0F;
    public float power = 10.0F;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Explode (GameObject player)
    {
        this.gameObject.transform.position = player.transform.position;
        player.SetActive(false);
        for (int i = 0; i < botPieces.Length; i++)
        {
            botPieces[i].SetActive(true);

            Rigidbody rb = botPieces[i].GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(power, this.gameObject.transform.position, radius, 3.0F);
        }
    }
    
}
