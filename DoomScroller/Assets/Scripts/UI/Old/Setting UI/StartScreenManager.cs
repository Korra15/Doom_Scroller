using UnityEngine;

public class StartScreenManager : MonoBehaviour
{
    public GameObject pressAnyButtonToStartText; // Assign your text GameObject here
    public GameObject[] mainMenuButtons; // Assign all your main menu buttons here

    // Update is called once per frame
    void Update()
    {
        // Detects if any key is pressed down
        if (Input.anyKeyDown)
        {
            // Disables the "Press Any Button To Start" text
            pressAnyButtonToStartText.SetActive(false);

            // Enables all the main menu buttons
            foreach (var button in mainMenuButtons)
            {
                button.SetActive(true);
            }

            // Disable this script to prevent it from running again
            this.enabled = false;
        }
    }
}
