using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inputs : MonoBehaviour
{
    public PlayerInput playerInput {  get; private set; }
    public PlayerInput.PlayerActions playerActions { get; private set; }

    public PlayerController controller { get; private set; }
    private void Awake()
    {
        playerInput = new PlayerInput();
        playerActions = playerInput.Player;
        controller = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.Player.Move.performed += controller.OnMoveInputPerformed;
        playerInput.Player.Move.canceled += controller.OnMoveInputCanceled;
        playerInput.Player.Look.performed += controller.OnLookInputPerformed;
        playerInput.Player.Look.canceled += controller.OnLookInputCanceled;
        playerInput.Player.Jump.started += controller.OnJumpInput;
    }

    private void OnDisable()
    {
        playerInput.Disable();
        playerInput.Player.Move.performed -= controller.OnMoveInputPerformed;
        playerInput.Player.Move.canceled -= controller.OnMoveInputCanceled;
        playerInput.Player.Look.performed -= controller.OnLookInputPerformed;
        playerInput.Player.Look.canceled -= controller.OnLookInputCanceled;
        playerInput.Player.Jump.started -= controller.OnJumpInput;

    }
}
