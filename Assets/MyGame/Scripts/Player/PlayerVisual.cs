using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private PlayerLocomotion playerLocomotion;
    private PlayerAttack playerAttack;
    private PlayerHealth playerHealth;
    private GameInput gameInput;
    private Animator animator;

    private const string IS_WALKING = "Walk";
    private const string IS_IDLE = "Idle";
    private const string IS_RUNNING = "Running";
    private const string IS_ATTACK = "Punch";
    private const string IS_BLOCK = "Block";
    private const string IS_PARRY = "Parry";
    private const string IS_HIT = "Hit";

    private float walkingDuration = 0f;
    private float runningDuration = 0f;
    private float idleDuration = 0f;
    private float attackDuration = 0f;




    private void Start()
    {
        playerLocomotion = GetComponentInParent<PlayerLocomotion>();
        playerAttack = GetComponentInParent<PlayerAttack>();
        gameInput = GetComponentInParent<GameInput>();
        playerHealth = GetComponentInParent<PlayerHealth>();
        animator = GetComponent<Animator>();

        playerLocomotion.OnMoveChanged += PlayerLocomotion_OnMoveChanged;
        playerAttack.OnPlayerAttack += PlayerAttack_OnPlayerAttack;
        playerHealth.OnPlayerBlock += PlayerHealth_OnPlayerBlock;
        playerHealth.OnPlayerBlockStop += PlayerHealth_OnPlayerBlockStop;
        playerHealth.OnPlayerHit += PlayerHealth_OnPlayerHit;
        playerHealth.OnPlayerParrySuccess += PlayerHealth_OnPlayerParrySuccess;
    }

    private void PlayerHealth_OnPlayerParrySuccess(object sender, EventArgs e)
    {
        animator.CrossFade(IS_PARRY, 0f);
    }

    private void PlayerHealth_OnPlayerHit(object sender, EventArgs e)
    {
        animator.CrossFade(IS_HIT, 0f, 0, 0);
    }

    private void PlayerHealth_OnPlayerBlockStop(object sender, EventArgs e)
    {
        HandleLocomotion();
    }

    private void PlayerHealth_OnPlayerBlock(object sender, EventArgs e)
    {
        animator.CrossFade(IS_BLOCK, 0f);
    }

    private void PlayerAttack_OnPlayerAttack(int index)
    {
        animator.CrossFade(IS_ATTACK + index.ToString(), attackDuration);
        walkingDuration = 0.1f;
        runningDuration = 0.1f;
    }

    private void PlayerLocomotion_OnMoveChanged(object sender, System.EventArgs e)
    {
        HandleLocomotion();
    }

    private void HandleLocomotion()
    {
        if (playerLocomotion.IsWalking())
        {
            if (playerLocomotion.IsRunning())
            {
                animator.CrossFade(IS_RUNNING, 0f);
                walkingDuration = 0.4f;
                idleDuration = 0f;
                attackDuration = 0.1f;
            }
            else
            {
                animator.CrossFade(IS_WALKING, walkingDuration);
                runningDuration = 0.4f;
                idleDuration = 0.4f;
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

    private void ParryStateStart()
    {
        playerHealth.ChangeParryState(true);
    }
    private void ParryStateEnd()
    {
        playerHealth.ChangeParryState(false);
    }
    private void FinishPunch()
    {
        playerLocomotion.ChangeCanMove(true);
        HandleLocomotion();
        playerAttack.AttackPerform();
    }
}
