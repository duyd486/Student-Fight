using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Serialization;
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
    private const string IS_COMBAT_IDLE = "Combat Idle";
    private const string IS_RUNNING = "Running";
    private const string IS_ATTACK = "Punch";
    private const string IS_BLOCK = "Block";
    private const string IS_PARRY = "Parry";
    private const string IS_HIT = "Hit";
    private const string IS_QUICK_ATK = "Quick Punch";

    private float walkingDuration = 0f;
    private float runningDuration = 0f;
    private float idleDuration = 0f;
    private float attackDuration = 0f;

    private bool isBeingHit = false;
    private bool isParrying = false;




    private void Start()
    {
        playerLocomotion = GetComponentInParent<PlayerLocomotion>();
        playerAttack = GetComponentInParent<PlayerAttack>();
        gameInput = GetComponentInParent<GameInput>();
        playerHealth = GetComponentInParent<PlayerHealth>();
        animator = GetComponent<Animator>();

        playerLocomotion.OnMoveChanged += PlayerLocomotion_OnMoveChanged;
        playerAttack.OnPlayerAttack += PlayerAttack_OnPlayerAttack;
        playerAttack.OnPlayerQuickAtk += PlayerAttack_OnPlayerQuickAtk;
        playerHealth.OnPlayerBlock += PlayerHealth_OnPlayerBlock;
        playerHealth.OnPlayerBlockStop += PlayerHealth_OnPlayerBlockStop;
        playerHealth.OnPlayerHit += PlayerHealth_OnPlayerHit;
        playerHealth.OnPlayerParrySuccess += PlayerHealth_OnPlayerParrySuccess;
    }

    private void PlayerAttack_OnPlayerQuickAtk(object sender, EventArgs e)
    {
        animator.CrossFade(IS_QUICK_ATK, 0f);
        walkingDuration = 0.1f;
        runningDuration = 0.1f;
    }

    private void PlayerHealth_OnPlayerParrySuccess(object sender, EventArgs e)
    {
        animator.CrossFade(IS_PARRY, 0f);
        idleDuration = 0f;
        isParrying = true;
    }

    private void PlayerHealth_OnPlayerHit(object sender, EventArgs e)
    {
        playerLocomotion.ChangeCanMove(false);
        playerAttack.SetCanAttack(false);
        playerHealth.SetCanBlock(false);
        animator.CrossFade(IS_HIT, 0f, 0, 0);
        isBeingHit = true;
    }

    private void PlayerHealth_OnPlayerBlockStop(object sender, EventArgs e)
    {
        if(!isParrying)
        {
            idleDuration = 0.2f;
            HandleLocomotion();
        }
        isParrying = false;
    }

    private void PlayerHealth_OnPlayerBlock(object sender, EventArgs e)
    {
        animator.CrossFade(IS_BLOCK, 0f);
    }

    private void PlayerAttack_OnPlayerAttack(int index)
    {
        animator.CrossFade(IS_ATTACK + index.ToString(), attackDuration);
        walkingDuration = 0f;
        runningDuration = 0.1f;
        idleDuration = 0.1f;
    }

    private void PlayerLocomotion_OnMoveChanged(object sender, System.EventArgs e)
    {
        if (playerLocomotion.GetCanMove())
        {
            HandleLocomotion();
        }
    }

    private void HandleLocomotion()
    {
        if (!isBeingHit)
        {
            if (playerLocomotion.IsWalking())
            {
                if (playerLocomotion.IsRunning())
                {
                    animator.CrossFade(IS_RUNNING, runningDuration);
                    walkingDuration = 0.4f;
                    idleDuration = 0.6f;
                    attackDuration = 0.1f;
                }
                else
                {
                    animator.CrossFade(IS_WALKING, walkingDuration);
                    runningDuration = 0.4f;
                    idleDuration = 1f;
                    attackDuration = 0.1f;
                }
            }
            else
            {
                if(playerHealth.GetCurrentState() == PlayerHealth.State.Combat)
                {
                    animator.CrossFade(IS_COMBAT_IDLE, idleDuration);
                }
                else
                {
                    animator.CrossFade(IS_IDLE, idleDuration);
                }
                walkingDuration = 0f;
                runningDuration = 0f;
                attackDuration = 0.01f;
            }
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
    private void PunchPerform()
    {
        playerLocomotion.ChangeCanMove(true);
        playerAttack.AttackPerform();
        if(playerLocomotion.IsWalking() || playerLocomotion.IsRunning())
        {
            HandleLocomotion();
        }
    }
    private void PunchEnd()
    {
        HandleLocomotion();
    }
    private void FinishHit()
    {
        playerLocomotion.ChangeCanMove(true);
        playerAttack.SetCanAttack(true);
        playerHealth.SetCanBlock(true);
        isBeingHit = false;
    }
}
