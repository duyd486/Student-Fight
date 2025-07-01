using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentAI : MonoBehaviour, IDamageable
{
    [SerializeField] private Transform targetPoint;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float targetDistance = 20f;
    [SerializeField] private float moveToTargetDelay = 2f;
    [SerializeField] private float moveToTargetTimer = 0f;

    [SerializeField] private bool canMove = true;
    [SerializeField] private bool isWalking = false;
    [SerializeField] private bool isRunning = false;

    public event EventHandler OnMoveChanged;

    private void Update()
    {
        if (canMove)
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        if (targetPoint != null)
        {
            DebugDraw.DrawLine(targetPoint.position, transform.position, Color.yellow);

            if (Vector3.Distance(transform.position, targetPoint.position) > targetDistance && moveToTargetTimer < 0)
            {
                Vector3 direction = (targetPoint.position - transform.position).normalized;
                transform.position += moveSpeed * Time.deltaTime * direction;
                transform.forward = direction;
                if (!isWalking)
                {
                    OnMoveChanged?.Invoke(this, EventArgs.Empty);
                    isWalking = true;
                }
            } else
            {
                moveToTargetTimer -= Time.deltaTime;
                if (isWalking)
                {
                    OnMoveChanged?.Invoke(this, EventArgs.Empty);
                    isWalking = false;
                    moveToTargetTimer = moveToTargetDelay;
                }
            }
        }
    }

    public void TakeDamage(float damage, bool canParry = true)
    {
        Debug.Log("im being hit " + damage);
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
