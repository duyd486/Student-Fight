using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private PlayerLocomotion playerLocomotion;

    [SerializeField] private float timeBtwBlk = 0.8f;
    [SerializeField] private float timeBtwBlkTimer = 0f;

    public event EventHandler OnPlayerBlock;
    public event EventHandler OnPlayerBlockStop;

    private void Awake()
    {
        gameInput = GetComponent<GameInput>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void Start()
    {
        gameInput.OnBlockPress += GameInput_OnBlockPress;
        gameInput.OnBlockCancel += GameInput_OnBlockCancel;
    }

    private void Update()
    {
        timeBtwBlkTimer -= Time.deltaTime;
    }

    private void GameInput_OnBlockCancel(object sender, EventArgs e)
    {
        BlockCancel();
    }

    private void GameInput_OnBlockPress(object sender, EventArgs e)
    {
        BlockPerform();
    }



    private void BlockPerform()
    {
        if (timeBtwBlkTimer < 0f)
        {
            playerLocomotion.ChangeCanMove(false);
            OnPlayerBlock?.Invoke(this, EventArgs.Empty);
            timeBtwBlkTimer = timeBtwBlk;
        }
    }

    private async void BlockCancel()
    {
        playerLocomotion.ChangeCanMove(true);
        await Task.Delay(200);
        OnPlayerBlockStop?.Invoke(this, EventArgs.Empty);
    }
}
