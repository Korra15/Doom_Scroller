using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    // This boolean can be toggled in the Inspector.
    [SerializeField] private bool debugEnabled = true;

    private void Awake()
    {
        // Update the static variable based on the Inspector value at start.
        GlobalControl.DebugEnabled = debugEnabled;
    }

    private void OnValidate()
    {
        // Also update the static variable whenever the Inspector value changes.
        // This method is called in the editor when the script is loaded or a value is changed in the Inspector.
        GlobalControl.DebugEnabled = debugEnabled;
    }
}
