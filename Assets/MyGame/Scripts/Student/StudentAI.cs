﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class StudentAI : MonoBehaviour, IDamageable
{
    [SerializeField] private Transform targetPoint;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform hitPoint;
    [SerializeField] private NavMeshAgent agent;
    private Seat mySeat;

    public enum State
    {
        Combat,
        Default
    }
    private State state;

    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float targetDistance = 1f;
    [SerializeField] private float moveToTargetDelay = 2f;
    [SerializeField] private float moveToTargetTimer = 0f;

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
    public event EventHandler OnStudentSit;
    public event Action<int> OnStudentAttack;
    public event EventHandler OnStudentHit;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        state = State.Default;
    }

    private void Start()
    {
        agent.updateRotation = false;
        agent.avoidancePriority = UnityEngine.Random.Range(30, 70); // khác nhau cho mỗi AI
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;
        moveSpeed = UnityEngine.Random.Range(1, 3);
        GetSeat();
    }

    private void Update()
    {

        timeBtwTimer -= Time.deltaTime;
        comboTimer -= Time.deltaTime;

        if(targetPoint != null && canMove)
        {
            switch (state)
            {
                case State.Default:
                    HandleDefaultMovement();
                    break;
                case State.Combat:
                    HandleCombatMovement();
                    break;
            }
        }
        DebugDraw.Instance.DrawSphere(hitPoint.position, hitRadius, Color.red);
    }

    private void GetSeat()
    {
        if (School.Instance == null) return;
        if(School.Instance.GetSeat() != null)
        {
            Seat seat = School.Instance.GetSeat();
            SetTargetTransform(seat.transform);
            seat.SetStudentSeat();
            mySeat = seat;
            Debug.Log(mySeat.ToString());
        }
    }

    private void OutSeat()
    {
        if(mySeat == null) return;
        mySeat.SetStudentOut();
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
                target.TakeDamage(studentDamage, gameObject);
            }
        }
    }

    private void HandleDefaultMovement()
    {
        if (Vector3.Distance(transform.position, targetPoint.position) > 0.2f && moveToTargetTimer < 0)
        {
            // Di chuyen den vi tri target point
            agent.speed = moveSpeed;
            Vector3 direction = (agent.steeringTarget - transform.position).normalized;
            transform.forward = direction;
            if (!isWalking)
            {
                isWalking = true;
                OnMoveChanged?.Invoke(this, EventArgs.Empty);
            }
        } else
        {
            // Khi da den duoc vi tri
            moveToTargetTimer -= Time.deltaTime;
            transform.forward = targetPoint.right.normalized;
            if (isWalking)
            {
                isWalking = false;
                OnMoveChanged?.Invoke(this, EventArgs.Empty);
                OnStudentSit?.Invoke(this, EventArgs.Empty);
                moveToTargetTimer = UnityEngine.Random.Range(15f, 30f);
            }
            if (moveToTargetTimer < 0)
            {
                OutSeat();
                GetSeat();
            }
        }
    }

    private void HandleCombatMovement()
    {
        if (Vector3.Distance(transform.position, targetPoint.position) > targetDistance && moveToTargetTimer < 0)
        {
            // Di chuyen den vi tri target point
            SetTargetTransform(targetPoint);
            agent.speed = 4;
            Vector3 direction = (agent.steeringTarget - transform.position).normalized;
            transform.forward = direction;
            if (!isWalking)
            {
                isWalking = true;
                OnMoveChanged?.Invoke(this, EventArgs.Empty);
            }
        } else
        {
            // Khi da den duoc vi tri
            SetTargetTransform(null);
            ComboPerform();
            Vector3 direction = (targetPoint.position - transform.position).normalized;
            transform.forward = direction;
            moveToTargetTimer -= Time.deltaTime;
            if (isWalking)
            {
                isWalking = false;
                OnMoveChanged?.Invoke(this, EventArgs.Empty);
                moveToTargetTimer = moveToTargetDelay;
            }
        }
    }

    public void TakeDamage(float damage, GameObject attacker, bool canParry = true)
    {
        OnStudentHit?.Invoke(this, EventArgs.Empty);
        SetTargetTransform(attacker.transform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if(collision.gameObject.GetComponent<StudentAI>() != null)
        //{
        //    rb.AddForce(transform.right * 10f);

        //}
    }

    public bool IsWalking()
    {
        return isWalking;
    }
    public bool IsRunning()
    {
        return isRunning;
    }
    public void SetTargetTransform(Transform target)
    {
        if (target == null)
        {
            agent.ResetPath();
            return;
        }
        targetPoint = target;
        if(target.gameObject.GetComponent<IDamageable>() != null)
        {
            state = State.Combat;
            agent.SetDestination(targetPoint.position);
        } else
        {
            state = State.Default;
            agent.SetDestination(targetPoint.position);
        }
    }
    public State GetStudentState()
    {
        return state;
    }
}
