using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public class BigCharacterMovement : MonoBehaviour
{
    private float horizontal;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpPower = 7f;
    private bool isFacingRight = true;
    [SerializeField] private float circleRadius = 0.3f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private int attackDamage = 2;
    [SerializeField] private float attackRate = 3f;
    private float nextAttackTime = 0f;

    [SerializeField] private int playerHealth = 3;
    [SerializeField] private float invulnerabilityTime = 1f;
    private bool isPlayerInvulnerable = false;

    public GameObject[] hearts;

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

        if (Time.time > nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                nextAttackTime = Time.time + 1 / attackRate;
            }
        }

        Flip();
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

    private void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy") && !isPlayerInvulnerable)
        {
            // Example damage value; adjust as needed
            TakeDamage(1);
        }

        if (collision.collider.CompareTag("Spikes"))
        {
            for (var i = 1; i >= 0; i--)
            {
                Destroy(hearts[i].gameObject);
            }
            Die();
        }
    }

    private void TakeDamage(int damage)
    {
        if (isPlayerInvulnerable) return;

        playerHealth -= damage;
        Destroy(hearts[playerHealth].gameObject);

        if (playerHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(PlayerInvulnerabilityCoroutine());
        }
    }

    private IEnumerator PlayerInvulnerabilityCoroutine()
    {
        isPlayerInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityTime);
        isPlayerInvulnerable = false;
    }

    private void Die()
    {
        Debug.Log("Player died");
        this.enabled = false;
    }
}
