using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerLocomotion : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private float currentMoveSpeed;
    [SerializeField] private float defaultMoveSpeed = 7f;
    [SerializeField] private float runSpeed = 15f;
    [SerializeField] private float rotateSpeed = 12f;
    [SerializeField] private bool isWalking = false;
    [SerializeField] private bool isRunning = false;
    [SerializeField] private bool canMove = true;


    public event EventHandler OnMoveChanged;

    private void Awake()
    {
        gameInput = GetComponent<GameInput>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    private void Start()
    {
        gameInput.OnDodgeHold += GameInput_OnDodgeHold;
        gameInput.OnDodgeCancel += GameInput_OnDodgeCancel;
        playerAttack.OnPlayerAttack += PlayerAttack_OnPlayerAttack;

        currentMoveSpeed = defaultMoveSpeed;
    }

    private void PlayerAttack_OnPlayerAttack(int obj)
    {
        MoveDelay(800);
    }

    private async void MoveDelay(int milisecDelay)
    {
        canMove = false;
        await Task.Delay(milisecDelay);
        canMove = true;
        OnMoveChanged?.Invoke(this, EventArgs.Empty);
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
        if (canMove)
        {
            HandleMovement();
        }
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
    public void ChangeCanMove(bool canMove)
    {
        this.canMove = canMove;
    }
}
