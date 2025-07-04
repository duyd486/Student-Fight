using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private PlayerLocomotion playerLocomotion;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private IDamageable enemy;

    public enum State
    {
        Combat,
        Default
    }
    private State state;

    [SerializeField] private bool isParry = false;
    [SerializeField] private bool isBlocking = false;
    [SerializeField] private bool canBlock = true;
    [SerializeField] private float timeBtwBlk = 0.8f;
    [SerializeField] private float timeBtwBlkTimer = 0f;

    [SerializeField] private float combatStateMax = 10f;
    [SerializeField] private float combatStateTimer = 0;

    public event EventHandler OnPlayerBlock;
    public event EventHandler OnPlayerBlockStop;

    public event EventHandler OnPlayerHit;
    public event EventHandler OnPlayerParrySuccess;

    private void Awake()
    {
        gameInput = GetComponent<GameInput>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerAttack = GetComponent<PlayerAttack>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        gameInput.OnBlockPress += GameInput_OnBlockPress;
        gameInput.OnBlockCancel += GameInput_OnBlockCancel;
        gameInput.OnHitTestPress += GameInput_OnHitTestPress;

        playerAttack.OnPlayerAttack += PlayerAttack_OnPlayerAttack;
        playerAttack.OnPlayerQuickAtk += PlayerAttack_OnPlayerQuickAtk;
    }


    private void Update()
    {
        timeBtwBlkTimer -= Time.deltaTime;
        combatStateTimer -= Time.deltaTime;
        if(combatStateTimer < 0)
        {
            state = State.Default;
        }
    }
    private void PlayerAttack_OnPlayerAttack(int obj)
    {
        SetCurrentState(State.Combat);
    }
    private void PlayerAttack_OnPlayerQuickAtk(object sender, EventArgs e)
    {
        SetCurrentState(State.Combat);
    }

    private void GameInput_OnHitTestPress(object sender, EventArgs e)
    {
        TakeDamage(30);
    }
    private void GameInput_OnBlockCancel(object sender, EventArgs e)
    {
        BlockCancel();
    }

    private void GameInput_OnBlockPress(object sender, EventArgs e)
    {
        if (canBlock)
        {
            BlockPerform();
        }
    }


    public void TakeDamage(float damage, bool canParry = true)
    {
        Debug.Log("Player is taking damage");
        if (isParry)
        {
            HandleParrySuccess();
        }
        else if (isBlocking)
        {
            // Tru mau nhung it hon
            rb.AddForce(-transform.forward * damage * 3);
        }
        else
        {
            // Tru mau toan bo theo damage nhan vao
            OnPlayerHit?.Invoke(this, EventArgs.Empty);
            rb.AddForce(-transform.forward * damage * 4);
            isBlocking = false;
        }
        isParry = false;
        state = State.Combat;
        combatStateTimer = combatStateMax;
    }

    private void BlockPerform()
    {
        if (timeBtwBlkTimer < 0f)
        {
            playerLocomotion.ChangeCanMove(false);
            OnPlayerBlock?.Invoke(this, EventArgs.Empty);
            isBlocking = true;
            timeBtwBlkTimer = timeBtwBlk;
        }
    }

    private async void BlockCancel()
    {
        isBlocking = false;
        await Task.Delay(300);
        playerLocomotion.ChangeCanMove(true);
        OnPlayerBlockStop?.Invoke(this, EventArgs.Empty);
    }

    private async void HandleParrySuccess()
    {
        playerAttack.SetQuickAttackAble();
        isBlocking = false;
        playerLocomotion.ChangeCanMove(false);
        OnPlayerParrySuccess?.Invoke(this, EventArgs.Empty);
        await Task.Delay(100);
        playerLocomotion.ChangeCanMove(true);
    }

    public void ChangeParryState(bool state)
    {
        isParry = state;
    }
    public void SetCanBlock(bool canBlock)
    {
        this.canBlock = canBlock;
    }
    public State GetCurrentState()
    {
        return state;
    }
    public void SetCurrentState(State state)
    {
        this.state = state;
        combatStateTimer = combatStateMax;
    }
}
