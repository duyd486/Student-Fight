using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private PlayerLocomotion playerLocomotion;

    public event EventHandler OnAttackPress;

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

    private async void AttackPerform()
    {
        OnAttackPress?.Invoke(this, EventArgs.Empty);
        playerLocomotion.ChangeCanMove(false);
        await Task.Delay(1000);
        playerLocomotion.ChangeCanMove(true);
    }
}
