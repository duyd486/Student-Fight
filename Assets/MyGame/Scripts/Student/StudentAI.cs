using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentAI : MonoBehaviour, IDamageable
{
    [SerializeField] private Transform targetPoint;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float targetDistance = 1f;
    [SerializeField] private float moveToTargetDelay = 2f;
    [SerializeField] private float moveToTargetTimer = 0f;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform hitPoint;

    [SerializeField] private float studentDamage = 8f;
    [SerializeField] private float hitRadius = 0.5f;
    [SerializeField] private float timeBtwAtk = 1.3f;
    [SerializeField] private float comboTimer = 0f;
    [SerializeField] private float timeBtwTimer = 0f;
    [SerializeField] private int indexCombo = 1;
    [SerializeField] private float attackPush = 20f;

    [SerializeField] private bool canMove = true;
    [SerializeField] private bool isWalking = false;
    [SerializeField] private bool isRunning = false;

    public event EventHandler OnMoveChanged;
    public event Action<int> OnStudentAttack;
    public event EventHandler OnStudentHit;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        timeBtwTimer -= Time.deltaTime;
        comboTimer -= Time.deltaTime;
        if (canMove)
        {
            HandleMovement();
        }

        DebugDraw.Instance.DrawSphere(hitPoint.position, hitRadius, Color.red);
    }

    private void ComboPerform()
    {
        if (timeBtwTimer < 0f)
        {

            if (comboTimer > 0f)
            {
                indexCombo++;
            }
            else
            {
                indexCombo = 1;
            }
            if (indexCombo > 3)
            {
                indexCombo = 1;
            }

            OnStudentAttack?.Invoke(indexCombo);
            comboTimer = 4f;
            timeBtwTimer = timeBtwAtk;
            rb.AddForce(transform.forward * attackPush);
        }
        else return;
    }

    public void AttackPerform()
    {
        Collider[] hits = Physics.OverlapSphere(hitPoint.position, hitRadius);

        foreach (Collider hit in hits)
        {
            IDamageable target = hit.GetComponent<IDamageable>();

            if (target != null)
            {
                target.TakeDamage(studentDamage);
            }
        }
    }

    private void HandleMovement()
    {
        if (targetPoint != null)
        {
            DebugDraw.Instance.DrawLine(targetPoint.position, transform.position, Color.yellow);

            if (Vector3.Distance(transform.position, targetPoint.position) > targetDistance && moveToTargetTimer < 0)
            {
                Vector3 direction = (targetPoint.position - transform.position).normalized;
                transform.position += moveSpeed * Time.deltaTime * direction;
                transform.forward = direction;
                if (!isWalking)
                {
                    isWalking = true;
                    OnMoveChanged?.Invoke(this, EventArgs.Empty);
                }
            } else
            {
                ComboPerform();
                moveToTargetTimer -= Time.deltaTime;
                if (isWalking)
                {
                    isWalking = false;
                    OnMoveChanged?.Invoke(this, EventArgs.Empty);
                    moveToTargetTimer = moveToTargetDelay;
                }
            }
        }
    }

    public void TakeDamage(float damage, bool canParry = true)
    {
        OnStudentHit?.Invoke(this, EventArgs.Empty);
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
