using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class StudentAI : MonoBehaviour, IDamageable
{
    [SerializeField] private Transform targetPoint;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform hitPoint;
    [SerializeField] private NavMeshAgent agent;

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
        agent.SetDestination(targetPoint.position);
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        moveSpeed = UnityEngine.Random.Range(1, 3);
    }

    private void Update()
    {

        if(Input.GetMouseButtonDown(1))
        {
            Ray movePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(movePosition, out var hitInfo))
            {
                targetPoint.position = hitInfo.point;
                SetTargetTransform(targetPoint);
            }
        }

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
            if (isWalking)
            {
                isWalking = false;
                OnMoveChanged?.Invoke(this, EventArgs.Empty);
                moveToTargetTimer = moveToTargetDelay;
            }
        }
    }

    private void HandleCombatMovement()
    {
        if (Vector3.Distance(transform.position, targetPoint.position) > targetDistance && moveToTargetTimer < 0)
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

    public void TakeDamage(float damage, GameObject attacker, bool canParry = true)
    {
        OnStudentHit?.Invoke(this, EventArgs.Empty);
        SetTargetTransform(attacker.transform);
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
