using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision){
        GameObject other = collision.gameObject;
        if (other.CompareTag("Transition"))
        {
            LoadBachCavesScene();
        }
    }

    private void LoadBachCavesScene()
    {
        if(GlobalControl.DebugEnabled) Debug.Log("transition");
        SceneManager.LoadScene("Bach Caves"); // Replace with your actual scene name
    }
}