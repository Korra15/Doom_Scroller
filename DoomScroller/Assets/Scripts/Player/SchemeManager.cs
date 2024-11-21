using UnityEngine;
using UnityEngine.InputSystem;

public class SchemeManager : MonoBehaviour
{
    public GameObject keyboard;
    public GameObject playstation;
    public GameObject xbox;


    public PlayerInput playerInput;

    void Awake()
    {
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component is missing on this GameObject.");
            return;
        }

        // Subscribe to the control scheme changed event
        playerInput.onControlsChanged += OnControlSchemeChanged;
    }

    private void OnControlSchemeChanged(PlayerInput pi)
    {
        // Output the current control scheme
        Debug.Log("Control scheme changed to: " + pi.currentControlScheme);
        UpdateUIForScheme(pi.currentControlScheme);
    }

    private void UpdateUIForScheme(string scheme)
    {
        switch (scheme)
        {
            case "KeyboardMouse":
                ChangeMenuTo("Keyboard");
                break;
            case "Gamepad":
                // Further differentiation can be done here if needed
                IdentifyGamepadType();
                break;
            case "Touch":
                ChangeMenuTo("Touch");
                break;
            default:
                Debug.LogWarning("Unsupported control scheme: " + scheme);
                break;
        }
    }

    private void IdentifyGamepadType()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
        {
            Debug.LogError("Gamepad is expected but not found.");
            return;
        }

        if (gamepad.description.manufacturer.Contains("Sony"))
            ChangeMenuTo("PlayStation");
        else if (gamepad.description.manufacturer.Contains("Microsoft"))
            ChangeMenuTo("Xbox");
    }

    public void ChangeMenuTo(string deviceType)
    {
        //Debug.Log("Changing menu to: " + deviceType);
        // Implement your menu changing logic here.
        if(deviceType == "Keyboard")
        {
            keyboard.SetActive(true);
            playstation.SetActive(false);
            xbox.SetActive(false);
        }

        if(deviceType == "PlayStation") 
        {
            keyboard.SetActive(false);
            playstation.SetActive(true);
            xbox.SetActive(false);
        }

        if(deviceType == "Xbox")
        {
            keyboard.SetActive(false);
            playstation.SetActive(false);
            xbox.SetActive(true);
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from the event when the object is destroyed to avoid memory leaks
        if (playerInput != null)
        {
            playerInput.onControlsChanged -= OnControlSchemeChanged;
        }
    }
}
