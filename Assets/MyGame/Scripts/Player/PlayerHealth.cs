using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private PlayerLocomotion playerLocomotion;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private bool isParry = false;
    [SerializeField] private bool isBlocking = false;
    [SerializeField] private float timeBtwBlk = 0.8f;
    [SerializeField] private float timeBtwBlkTimer = 0f;

    public event EventHandler OnPlayerBlock;
    public event EventHandler OnPlayerBlockStop;

    public event EventHandler OnPlayerHit;
    public event EventHandler OnPlayerParrySuccess;

    private void Awake()
    {
        gameInput = GetComponent<GameInput>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        gameInput.OnBlockPress += GameInput_OnBlockPress;
        gameInput.OnBlockCancel += GameInput_OnBlockCancel;
        gameInput.OnHitTestPress += GameInput_OnHitTestPress;
    }


    private void Update()
    {
        timeBtwBlkTimer -= Time.deltaTime;
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
        BlockPerform();
    }


    public void TakeDamage(float damage)
    {
        if (isParry)
        {
            OnPlayerParrySuccess?.Invoke(this, EventArgs.Empty);
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
        await Task.Delay(200);
        playerLocomotion.ChangeCanMove(true);
        OnPlayerBlockStop?.Invoke(this, EventArgs.Empty);
    }

    public void ChangeParryState(bool state)
    {
        isParry = state;
    }
}
