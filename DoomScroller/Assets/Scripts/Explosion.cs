
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Explosion : MonoBehaviour
{
    public GameObject[] botPieces;
    public float radius;
    public float power;
    public GameObject explosionObject;
    // Start is called before the first frame update
    public void AddExplosionForce(Rigidbody2D rb, float explosionForce, Vector2 explosionPosition, float explosionRadius, float upwardsModifier = 0.5F, ForceMode2D mode = ForceMode2D.Force)
    {
        var explosionDir = rb.position - explosionPosition;
        var explosionDistance = explosionDir.magnitude;

        // Normalize without computing magnitude again
        if (upwardsModifier == 0)
            explosionDir /= explosionDistance;
        else
        {
            // From Rigidbody.AddExplosionForce doc:
            // If you pass a non-zero value for the upwardsModifier parameter, the direction
            // will be modified by subtracting that value from the Y component of the centre point.
            explosionDir.y += upwardsModifier;
            explosionDir.Normalize();
        }

        rb.AddForce(Mathf.Lerp(0, explosionForce, (1 - explosionDistance)) * explosionDir, mode);

    }


    // Update is called once per frame
    public void Explode(GameObject player)
    {

        this.gameObject.transform.position = player.transform.position;
        player.SetActive(false);
        for (int i = 0; i < botPieces.Length; i++)
        {
            botPieces[i].SetActive(true);

            Rigidbody2D rb = botPieces[i].GetComponent<Rigidbody2D>();

            AddExplosionForce(rb, power, this.gameObject.transform.position, radius);
        }
        explosionObject.transform.position = player.transform.position;

        explosionObject.SetActive(true);
        StartCoroutine(ReloadGame());
    }
    public IEnumerator ReloadGame()
    {

        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadSceneAsync(4);
    }

    
}