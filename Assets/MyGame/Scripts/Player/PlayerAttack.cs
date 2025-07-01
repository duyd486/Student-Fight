using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform hitPoint;

    [SerializeField] private float playerDamage = 15f;
    [SerializeField] private float hitRadius = 1f;
    [SerializeField] private float timeBtwAtk = 0.8f;
    [SerializeField] private float comboTimer = 0f;
    [SerializeField] private float timeBtwTimer = 0f;
    [SerializeField] private int indexCombo = 1;
    [SerializeField] private float attackPush = 20f;

    public event Action<int> OnPlayerAttack;

    private void Awake()
    {
        gameInput = GetComponent<GameInput>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        gameInput.OnAttackPress += GameInput_OnAttackPress;
    }

    private void Update()
    {
        timeBtwTimer -= Time.deltaTime;
        comboTimer -= Time.deltaTime;
        DebugDraw.DrawSphere(hitPoint.position, hitRadius, Color.red);
    }

    private void GameInput_OnAttackPress(object sender, System.EventArgs e)
    {
        ComboPerform();
    }

    private void ComboPerform()
    {
        if (timeBtwTimer < 0f)
        {

            if (comboTimer > 0f)
            {
                indexCombo++;
            }
            else
            {
                indexCombo = 1;
            }
            if (indexCombo > 3)
            {
                indexCombo = 1;
            }

            OnPlayerAttack?.Invoke(indexCombo);
            comboTimer = 2f;
            timeBtwTimer = timeBtwAtk;
            rb.AddForce(transform.forward * attackPush);
        }
        else return;
    }

    public void AttackPerform()
    {
        Collider[] hits = Physics.OverlapSphere(hitPoint.position, hitRadius);

        foreach (Collider hit in hits)
        {
            IDamageable target = hit.GetComponent<IDamageable>();

            if (target != null)
            {
                target.TakeDamage(playerDamage);
            }
        }
    }
}
