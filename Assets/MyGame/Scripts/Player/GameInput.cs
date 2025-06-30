using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{

    private PlayerInput playerInput;

    public event EventHandler OnDodgePress;
    public event EventHandler OnDodgeHold;
    public event EventHandler OnDodgeCancel;
    public event EventHandler OnAttackPress;
    public event EventHandler OnBlockPress;

    public event EventHandler OnBlockCancel;

    public event EventHandler OnHitTestPress;

    private void Awake()
    {
        playerInput = new PlayerInput();
        Cursor.lockState = CursorLockMode.None;
        InputSystem.EnableDevice(Mouse.current);

        playerInput.Player.Enable();
        playerInput.Player.Attack.Enable();
        playerInput.Player.Block.Enable();
        playerInput.Player.Dodge.started += Dodge_started;
        playerInput.Player.Dodge.performed += Dodge_performed;
        playerInput.Player.Dodge.canceled += Dodge_canceled;
        playerInput.Player.Attack.performed += Attack_performed;
        playerInput.Player.Block.started += Block_started;
        playerInput.Player.Block.performed += Block_performed;
        playerInput.Player.Block.canceled += Block_canceled;

        playerInput.Player.HitTest.performed += HitTest_performed;
    }

    private void HitTest_performed(InputAction.CallbackContext obj)
    {
        OnHitTestPress?.Invoke(this, EventArgs.Empty);
    }

    private void Block_canceled(InputAction.CallbackContext obj)
    {
        OnBlockCancel?.Invoke(this, EventArgs.Empty);
    }

    private void Block_performed(InputAction.CallbackContext obj)
    {
        Debug.Log("Block Perform");
    }

    private void Block_started(InputAction.CallbackContext obj)
    {
        OnBlockPress?.Invoke(this, EventArgs.Empty);
    }

    private void Dodge_started(InputAction.CallbackContext obj)
    {
        OnDodgePress?.Invoke(this, EventArgs.Empty);
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
