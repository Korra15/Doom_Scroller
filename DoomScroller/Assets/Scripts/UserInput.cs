using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    public static UserInput instance;

    public Vector2 MoveInput { get; private set; }
    // public Vector3 ClawMove { get; private set; }
    public bool Attack { get; private set; }
    public bool AttackHeld { get; private set; }
    public bool AttackReleased { get; private set; }
    public bool Block { get; private set; }
    public bool BlockHeld { get; private set; }
    public bool BlockReleased { get; private set; }
    public bool Dash { get; private set; }
    public bool DashHeld { get; private set; }
    public bool DashReleased { get; private set; }
    public bool Jump { get; private set; }
    public bool JumpHeld { get; private set; }
    public bool JumpReleased { get; private set; }
    public bool Crouch { get; private set; }
    public bool CrouchHeld { get; private set; }
    public bool CrouchReleased { get; private set; }
    public bool Sprint { get; private set; }
    public bool SprintHeld { get; private set; }
    public bool SprintReleased { get; private set; }
    public bool Glide { get; private set; }
    public bool GlideHeld { get; private set; }
    public bool GlideReleased { get; private set; }
    public bool MenuOpenClose { get; private set; }
    public bool Pause { get; private set; }
    public Vector2 Navigate { get; private set; }
    public bool Click { get; private set; }
    public bool Back { get; private set; }
    public bool Hud { get; private set; }
    public bool UI_InGameMenuOpenClose { get; private set; }
    public bool Interact { get; private set; }
    public bool FlipLeft { get; private set; }
    public bool FlipRight { get; private set; }

    private PlayerInput _playerInput;

    private InputAction _moveAction;
    // private InputAction _clawMove;
    private InputAction _attackAction;
    private InputAction _blockAction;
    private InputAction _dashAction;
    private InputAction _jumpAction;
    private InputAction _crouchAction;
    private InputAction _sprintAction;
    private InputAction _glideAction;
    private InputAction _menuOpenCloseAction;
    private InputAction _pauseAction;
    private InputAction _navigate;
    private InputAction _click;
    private InputAction _hud;
    private InputAction _back;
    private InputAction _UI_InGameMenuOpenClose;
    private InputAction _interactAction;
    private InputAction _flipLeft;
    private InputAction _flipRight;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _playerInput = GetComponent<PlayerInput>();

        SetupInputActions();

    }

    // Update is called once per frame 
    void Update()
    {
        UpdateInputs();
    }

    private void SetupInputActions()
    {
        _moveAction = _playerInput.actions["Move"];
        // _clawMove = _playerInput.actions["ClawMove"];
        _attackAction = _playerInput.actions["Attack"];
        _blockAction = _playerInput.actions["Block"];
        _dashAction = _playerInput.actions["Dash"];
        _jumpAction = _playerInput.actions["Jump"];
        _crouchAction = _playerInput.actions["Crouch"];
        _sprintAction = _playerInput.actions["Sprint"];
        _glideAction = _playerInput.actions["Glide"];
        _menuOpenCloseAction = _playerInput.actions["MenuOpenClose"];
        _pauseAction = _playerInput.actions["Pause"];
        _navigate = _playerInput.actions["Navigate"];
        _click = _playerInput.actions["Click"];
        _back = _playerInput.actions["Back"];
        _UI_InGameMenuOpenClose = _playerInput.actions["UI_InGameMenuOpenClose"];
        _hud = _playerInput.actions["Hud"];
        _interactAction = _playerInput.actions["Interact"];
        _flipLeft = _playerInput.actions["FlipLeft"];
        _flipRight = _playerInput.actions["FlipRight"];
    }

    private void UpdateInputs()
    {
        MoveInput = _moveAction.ReadValue<Vector2>();
        // ClawMove = _clawMove.ReadValue<Vector3>();
        Attack = _attackAction.WasPressedThisFrame();
        AttackHeld = _attackAction.IsPressed();
        AttackReleased = _attackAction.WasReleasedThisFrame();
        Block = _blockAction.WasPressedThisFrame();
        BlockHeld = _blockAction.IsPressed();
        BlockReleased = _blockAction.WasReleasedThisFrame();
        Dash = _dashAction.WasPressedThisFrame();
        DashHeld = _dashAction.IsPressed();
        DashReleased = _dashAction.WasReleasedThisFrame();
        Jump = _jumpAction.WasPressedThisFrame();
        JumpHeld = _jumpAction.IsPressed();
        JumpReleased = _jumpAction.WasReleasedThisFrame();
        Crouch = _crouchAction.WasPressedThisFrame();
        CrouchHeld = _crouchAction.IsPressed();
        CrouchReleased = _crouchAction.WasReleasedThisFrame();
        Sprint = _sprintAction.WasPressedThisFrame();
        SprintHeld = _sprintAction.IsPressed();
        SprintReleased = _sprintAction.WasReleasedThisFrame();
        Glide = _glideAction.WasPressedThisFrame();
        GlideHeld = _glideAction.IsPressed();
        GlideReleased = _glideAction.WasReleasedThisFrame();
        MenuOpenClose = _menuOpenCloseAction.WasPressedThisFrame();
        Pause = _pauseAction.WasPressedThisFrame();
        Navigate = _navigate.ReadValue<Vector2>();
        Click = _click.WasPressedThisFrame();
        Hud = _hud.WasPressedThisFrame();
        Back = _back.WasPressedThisFrame();
        UI_InGameMenuOpenClose = _UI_InGameMenuOpenClose.WasPressedThisFrame();
        Interact = _interactAction.WasPressedThisFrame();
        FlipLeft = _flipLeft.WasPerformedThisFrame();
        FlipRight = _flipRight.WasPerformedThisFrame();

    }

    public void UIActionMapEnable()
    {
        _playerInput.SwitchCurrentActionMap("UI");

    }

    public void inGameActionMapEnable()
    {
        _playerInput.SwitchCurrentActionMap("inGame");
    }

}
