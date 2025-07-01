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
        gameInput.OnDodgePress += GameInput_OnDodgePress;
        gameInput.OnDodgeHold += GameInput_OnDodgeHold;
        gameInput.OnDodgeCancel += GameInput_OnDodgeCancel;
        playerAttack.OnPlayerAttack += PlayerAttack_OnPlayerAttack;

        currentMoveSpeed = defaultMoveSpeed;
    }

    private void PlayerAttack_OnPlayerAttack(int obj)
    {
        canMove = false;
    }

    private void GameInput_OnDodgePress(object sender, EventArgs e)
    {
        isRunning = true;
        if (isWalking)
        {
            OnMoveChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInput_OnDodgeCancel(object sender, EventArgs e)
    {
        if (isWalking)
        {
            OnMoveChanged?.Invoke(this, EventArgs.Empty);
        }
        isRunning = false;
    }

    private void GameInput_OnDodgeHold(object sender, EventArgs e)
    {
        //currentMoveSpeed = runSpeed;
        //isRunning = true;
        //if (isWalking)
        //{
        //    OnMoveChanged?.Invoke(this, EventArgs.Empty);
        //}
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
        if (isRunning)
        {
            currentMoveSpeed = runSpeed;
        } else
        {
            currentMoveSpeed = defaultMoveSpeed;
        }
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

    private async void HandleDodge()
    {
        isRunning = true;
        if (isWalking)
        {
            OnMoveChanged?.Invoke(this, EventArgs.Empty);
        }
        currentMoveSpeed = currentMoveSpeed * 4;
        await Task.Delay(200);
        if (isRunning) {
            currentMoveSpeed = runSpeed;
        } else
        {
            currentMoveSpeed = defaultMoveSpeed;
        }
        
    }

    public bool IsWalking()
    {
        return isWalking;
    }
    public bool IsRunning()
    {
        return isRunning;
    }
    public bool GetCanMove()
    {
        return canMove;
    }
    public void ChangeCanMove(bool canMove)
    {
        this.canMove = canMove;
    }
}
