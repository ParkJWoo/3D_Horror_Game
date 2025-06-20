﻿using UnityEngine;

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
        playerInput.Player.Run.performed += controller.OnRunInputPerformed;
        playerInput.Player.Run.canceled += controller.OnRunInputCanceled;
        playerInput.Player.Inventory.started += controller.OnInventoryStarted;
        playerInput.Player.Interaction.started += controller.OnInteractionStarted;
        playerInput.Player.Flash.started += controller.OnFlashStarted;
        playerInput.Player.Menu.started += controller.OnMenu;
    }

    private void OnDisable()
    {
        playerInput.Disable();
        playerInput.Player.Move.performed -= controller.OnMoveInputPerformed;
        playerInput.Player.Move.canceled -= controller.OnMoveInputCanceled;
        playerInput.Player.Look.performed -= controller.OnLookInputPerformed;
        playerInput.Player.Look.canceled -= controller.OnLookInputCanceled;
        playerInput.Player.Jump.started -= controller.OnJumpInput;
        playerInput.Player.Run.performed -= controller.OnRunInputPerformed;
        playerInput.Player.Run.canceled -= controller.OnRunInputCanceled;
        playerInput.Player.Inventory.started -= controller.OnInventoryStarted;
        playerInput.Player.Interaction.started -= controller.OnInteractionStarted;
        playerInput.Player.Flash.started -= controller.OnFlashStarted;
        playerInput.Player.Menu.started -= controller.OnMenu;

    }
}
