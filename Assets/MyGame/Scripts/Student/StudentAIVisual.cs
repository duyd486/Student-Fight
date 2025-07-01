using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentAIVisual : MonoBehaviour
{
    [SerializeField] private StudentAI studentAI;

    [SerializeField] private Animator animator;

    private const string IS_WALKING = "Walk";
    private const string IS_IDLE = "Idle";
    private const string IS_RUNNING = "Running";
    private const string IS_ATTACK = "Punch";
    private const string IS_BLOCK = "Block";
    private const string IS_PARRY = "Parry";
    private const string IS_HIT = "Hit";

    private void Awake()
    {
        studentAI = GetComponentInParent<StudentAI>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleLocomotion();
    }

    private void HandleLocomotion()
    {
        if (studentAI.IsWalking())
        {
            if (studentAI.IsRunning())
            {
                animator.CrossFade(IS_RUNNING, 0f);
            }
            else
            {
                //animator.CrossFade(IS_WALKING, 0f);
                animator.CrossFade(IS_RUNNING, 0f);
            }
        }
        else
        {
            animator.CrossFade(IS_IDLE, 0f);
        }
    }
}
