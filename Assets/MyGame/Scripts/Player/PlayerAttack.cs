using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private PlayerLocomotion playerLocomotion;

    public event EventHandler OnPlayerAttack;

    private void Awake()
    {
        gameInput = GetComponent<GameInput>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void Start()
    {
        gameInput.OnAttackPress += GameInput_OnAttackPress;
    }

    private void GameInput_OnAttackPress(object sender, System.EventArgs e)
    {
        AttackPerform();
    }

    private void AttackPerform()
    {
        OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }
}
