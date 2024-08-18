using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;

public class SmallCharacterMovement : MonoBehaviour
{
    private float horizontal;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpPower = 15f;
    private bool isFacingRight = true;
    [SerializeField] private float circleRadius = 0.1f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;


    public Transform attackPoint;
    public float attackRange;
    public LayerMask enemyLayers;
    public int attackDamage = 1;


    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        // Jump

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpPower);
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocityY > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY * 0.05f);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Attack();
        }

        Flip();
    }

    private void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyDamage>().TakeDamage(attackDamage);
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocityY);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, circleRadius, groundLayer);
    }
}
