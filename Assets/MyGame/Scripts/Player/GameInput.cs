using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{

    private PlayerInput playerInput;

    public event EventHandler OnDodgeHold;
    public event EventHandler OnDodgeCancel;
    public event EventHandler OnAttackPress;

    private void Awake()
    {
        playerInput = new PlayerInput();
        Cursor.lockState = CursorLockMode.None;
        InputSystem.EnableDevice(Mouse.current);

        playerInput.Player.Enable();
        playerInput.Player.Attack.Enable();
        playerInput.Player.Dodge.performed += Dodge_performed;
        playerInput.Player.Dodge.canceled += Dodge_canceled;
        playerInput.Player.Attack.performed += Attack_performed;
    }

    private void Attack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnAttackPress?.Invoke(this, EventArgs.Empty);
    }

    private void Dodge_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnDodgeCancel?.Invoke(this, EventArgs.Empty);
    }

    private void Dodge_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnDodgeHold?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInput.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;

    }

}
