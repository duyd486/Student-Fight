using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private PlayerLocomotion playerLocomotion;
    private PlayerAttack playerAttack;
    private Animator animator;

    private const string IS_WALKING = "Walk";
    private const string IS_IDLE = "Idle";
    private const string IS_RUNNING = "Running";
    private const string IS_ATTACK = "Punch";

    private float walkingDuration = 0f;
    private float runningDuration = 0f;
    private float idleDuration = 0f;




    private void Start()
    {
        playerLocomotion = GetComponentInParent<PlayerLocomotion>();
        playerAttack = GetComponentInParent<PlayerAttack>();
        animator = GetComponent<Animator>();

        playerLocomotion.OnMoveChanged += PlayerLocomotion_OnMoveChanged;
        playerAttack.OnAttackPress += PlayerAttack_OnAttackPress;
    }

    private void PlayerAttack_OnAttackPress(object sender, System.EventArgs e)
    {
        animator.CrossFade(IS_ATTACK, 0f);
    }

    private void PlayerLocomotion_OnMoveChanged(object sender, System.EventArgs e)
    {

        if (playerLocomotion.IsWalking())
        {

            if (playerLocomotion.IsRunning())
            {
                animator.CrossFade(IS_RUNNING, runningDuration);
                walkingDuration = 0.4f;
                idleDuration = 0f;
            }
            else
            {
                animator.CrossFade(IS_WALKING, walkingDuration);
                runningDuration = 0.4f;
                idleDuration = 0.5f;
            }
        }
        else
        {
            animator.CrossFade(IS_IDLE, idleDuration);
            walkingDuration = 0f;
            runningDuration = 0f;
        }
    }
}
