using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private PlayerInput playerInput;

    public event EventHandler OnDodgeHold;
    public event EventHandler OnDodgeCancel;

    private void Awake()
    {
        Instance = this;
        playerInput = new PlayerInput();

        playerInput.Player.Enable();
        playerInput.Player.Dodge.performed += Dodge_performed;
        playerInput.Player.Dodge.canceled += Dodge_canceled;
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
