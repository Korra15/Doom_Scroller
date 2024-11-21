using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance; //making this singleton

    public bool InstructionUIOpenCloseInput { get; private set; }

    private PlayerInput _playerInput;
    private InputAction _instructionUIOpenCloseAction;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _playerInput = GetComponent<PlayerInput>();
        _instructionUIOpenCloseAction = _playerInput.actions["InstructionUIOpenClose"];
    }

    private void Update()
    {
        
        InstructionUIOpenCloseInput = _instructionUIOpenCloseAction.WasPressedThisFrame();
      
    }
}
