using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsDone : MonoBehaviour
{
    public void OnDoneButtonPressed()
    {
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with the exact name of your main menu scene
    }
}
