using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentAI : MonoBehaviour, IDamageable
{
    [SerializeField] private Transform targetPoint;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float targetDistance = 20f;

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (targetPoint != null)
        {
            DebugDraw.DrawLine(targetPoint.position, transform.position, Color.yellow);

            if (Vector3.Distance(transform.position, targetPoint.position) > targetDistance)
            {
                Vector3 direction = (targetPoint.position - transform.position).normalized;
                transform.position += moveSpeed * Time.deltaTime * direction;
                transform.forward = direction;
            }
        }
    }

    public void TakeDamage(float damage, bool canParry = true)
    {
        Debug.Log("im being hit " + damage);
    }
}
