using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerLocomotion : MonoBehaviour
{
    [SerializeField] private float currentMoveSpeed;
    [SerializeField] private float defaultMoveSpeed = 7f;
    [SerializeField] private float runSpeed = 15f;
    [SerializeField] private float rotateSpeed = 12f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private bool isWalking = false;
    [SerializeField] private bool isRunning = false;


    public event EventHandler OnMoveChanged;

    private void Start()
    {
        GameInput.Instance.OnDodgeHold += GameInput_OnDodgeHold;
        GameInput.Instance.OnDodgeCancel += GameInput_OnDodgeCancel;

        currentMoveSpeed = defaultMoveSpeed;
    }

    private void GameInput_OnDodgeCancel(object sender, EventArgs e)
    {
        currentMoveSpeed = defaultMoveSpeed;
        isRunning = false;
        OnMoveChanged?.Invoke(this, EventArgs.Empty);
    }

    private void GameInput_OnDodgeHold(object sender, EventArgs e)
    {
        currentMoveSpeed = runSpeed;
        isRunning = true;
        OnMoveChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        float moveDistance = currentMoveSpeed * Time.deltaTime;

        Transform cameraTransform = Camera.main.transform;
        Vector3 inputDir = new Vector3(inputVector.x, 0f, inputVector.y);
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDir = (cameraForward * inputDir.z + cameraRight * inputDir.x).normalized;

        transform.position += moveDir * moveDistance;


        if((moveDir != Vector3.zero) != isWalking)
        {
            isWalking = moveDir != Vector3.zero;
            OnMoveChanged?.Invoke(this, EventArgs.Empty);
        }

        isWalking = moveDir != Vector3.zero;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }
    public bool IsWalking()
    {
        return isWalking;
    }
    public bool IsRunning()
    {
        return isRunning;
    }
}
