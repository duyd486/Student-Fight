using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private PlayerLocomotion playerLocomotion;
    private PlayerAttack playerAttack;
    private GameInput gameInput;
    private Animator animator;

    private const string IS_WALKING = "Walk";
    private const string IS_IDLE = "Idle";
    private const string IS_RUNNING = "Running";
    private const string IS_ATTACK = "Punch";

    private float walkingDuration = 0f;
    private float runningDuration = 0f;
    private float idleDuration = 0f;
    private float attackDuration = 0f;




    private void Start()
    {
        playerLocomotion = GetComponentInParent<PlayerLocomotion>();
        playerAttack = GetComponentInParent<PlayerAttack>();
        gameInput = GetComponentInParent<GameInput>();
        
        animator = GetComponent<Animator>();

        playerLocomotion.OnMoveChanged += PlayerLocomotion_OnMoveChanged;
        playerAttack.OnPlayerAttack += PlayerAttack_OnPlayerAttack;
    }

    private void PlayerAttack_OnPlayerAttack(int index)
    {
        animator.CrossFade(IS_ATTACK + index.ToString(), attackDuration);
        walkingDuration = 0.1f;
        runningDuration = 0.1f;
        Debug.Log(IS_ATTACK + index.ToString());
    }

    private void PlayerLocomotion_OnMoveChanged(object sender, System.EventArgs e)
    {

        if (playerLocomotion.IsWalking())
        {

            if (playerLocomotion.IsRunning())
            {
                animator.CrossFade(IS_RUNNING, 0f);
                walkingDuration = 0.4f;
                idleDuration = 0.5f;
                attackDuration = 0.1f;
            }
            else
            {
                animator.CrossFade(IS_WALKING, walkingDuration);
                runningDuration = 0.4f;
                idleDuration = 0.5f;
                attackDuration = 0.1f;
            }
        }
        else
        {
            animator.CrossFade(IS_IDLE, idleDuration);
            walkingDuration = 0f;
            runningDuration = 0f;
            attackDuration = 0.01f;
        }
    }
}
