// using UnityEngine;
// using UnityEngine.SceneManagement;
// using System.Collections;

// public class SceneTransition : MonoBehaviour
// {
//     public Animator animator; // Assign the animator of your "Studio Name Presents" text
//     public string mainMenuSceneName = "MainMenu"; // Replace with the name of your main menu scene
//     // public float delayBeforeTransition = 2.0f;


//     private void BeginTransition()
//     {
        
//         // Start the transition after the animation ends
//         StartCoroutine(TransitionAfterAnimation());
//     }

//     // private IEnumerator StartTransitionAfterDelay()
//     // {
//     //     // Wait for the specified delay
//     //     yield return new WaitForSeconds(delayBeforeTransition);

//     //     // Start the transition animation
//     //     StartCoroutine(TransitionAfterAnimation());
//     // }


//     private IEnumerator TransitionAfterAnimation()
//     {
        
//         // Wait for the animation clip to finish
//         // Make sure the name "StudioNameFade" matches the name of your animation clip
//         yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("StudioNameFade") &&
//                                          animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        
//         yield return new WaitForSeconds(1.0f);

//         // Load the main menu scene
//         SceneManager.LoadScene(mainMenuSceneName);
//     }

    
// }
