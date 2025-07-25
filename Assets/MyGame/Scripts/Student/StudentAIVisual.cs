using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentAIVisual : MonoBehaviour
{
    [SerializeField] private StudentAI studentAI;

    [SerializeField] private Animator animator;

    private const string IS_WALKING = "Walk";
    private const string IS_IDLE = "Idle";
    private const string IS_COMBAT_IDLE = "Combat Idle";
    private const string IS_RUNNING = "Running";
    private const string IS_ATTACK = "Punch";
    private const string IS_BLOCK = "Block";
    private const string IS_PARRY = "Parry";
    private const string IS_HIT = "Hit";
    private const string IS_SIT = "Sit";

    private void Awake()
    {
        studentAI = GetComponentInParent<StudentAI>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        studentAI.OnMoveChanged += StudentAI_OnMoveChanged;
        studentAI.OnStudentAttack += StudentAI_OnStudentAttack;
        studentAI.OnStudentHit += StudentAI_OnStudentHit;
        studentAI.OnStudentSit += StudentAI_OnStudentSit;
    }

    private void StudentAI_OnStudentSit(object sender, EventArgs e)
    {
        animator.CrossFade(IS_SIT, 0f);
    }

    private void StudentAI_OnStudentHit(object sender, EventArgs e)
    {
        animator.CrossFade(IS_HIT, 0f, 0, 0);
    }

    private void StudentAI_OnMoveChanged(object sender, EventArgs e)
    {
        HandleLocomotion();
    }

    private void StudentAI_OnStudentAttack(int index)
    {
        animator.CrossFade(IS_ATTACK + index.ToString(), 0f);
    }

    private void HandleLocomotion()
    {
        switch (studentAI.GetStudentState())
        {
            case StudentAI.State.Default:
                if (studentAI.IsWalking())
                {
                    animator.CrossFade(IS_WALKING, 0f);
                }
                else
                {
                    animator.CrossFade(IS_IDLE, 0f);
                }
                break;
            case StudentAI.State.Combat:
                if (studentAI.IsWalking())
                {
                    animator.CrossFade(IS_RUNNING, 0f);
                }
                else
                {
                    animator.CrossFade(IS_COMBAT_IDLE, 0f);
                }
                break;
        }
    }

    private void ParryStateStart()
    {
        Debug.Log("Parry start on student ai");
    }
    private void ParryStateEnd()
    {
        Debug.Log("Parry end on student ai");
    }
    private void PunchPerform()
    {
        studentAI.AttackPerform();
    }
    private void PunchEnd()
    {
        HandleLocomotion();
    }
    private void FinishHit()
    {
        
    }
}
