using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;


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
    [SerializeField] private GameObject attackEffect;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private int attackDamage = 2;
    [SerializeField] private float attackRate = 3f;
    private float nextAttackTime = 2.5f;


    [SerializeField] private float playerHealth = 3f;
    [SerializeField] private float currentHealth;
    [SerializeField] private float invulnerabilityTime = 3f;
    private bool isPlayerInvulnerable = false;

    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image currentHealthBar;

    public Animator animator;
    public GameObject fireBall;

    private void Update()
    {
        //Animation things
        if (horizontal != 0f)
        {
            animator.SetBool("AnimBool_IsWalking", true);
        }
        else
        {
            animator.SetBool("AnimBool_IsWalking", false);
        }
        if (IsGrounded() == true)
        {
            animator.SetBool("AnimBool_IsGrounded", true);
        }
        else
        {
            animator.SetBool("AnimBool_IsGrounded", false);
        }

        horizontal = Input.GetAxisRaw("Horizontal");


        // --------Animation-related-------- 
        if (horizontal != 0f)
        {
            animator.SetBool("AnimBool_isWalking", true);
        }
        else
        {
            animator.SetBool("AnimBool_isWalking", false);
        }
        if (IsGrounded() == true)
        {
            animator.SetBool("AnimBool_isGrounded", true);
        }
        else
        {
            animator.SetBool("AnimBool_isGrounded", false);
        }
        animator.SetFloat("AnimFloat_YVelocity", rb.linearVelocityY);
        // --------End--------

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpPower);

        }
        if (Input.GetButtonUp("Jump") && rb.linearVelocityY > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY * 0.05f);
            animator.SetTrigger("JumpAnimation");
        }

        if (Time.time > nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (IsGrounded() == true)
                {
                    animator.SetTrigger("AnimTrig_AttackGrounded");
                }
                else
                {
                    animator.SetTrigger("AnimTrig_AttackAirborn");
                }
                Attack();
                nextAttackTime = Time.time + 3 / attackRate;
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
        animator.SetTrigger("AttackAnimation");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }

        StartCoroutine(DisableAttackEffect());
    }

    private IEnumerator DisableAttackEffect()
    {
        yield return new WaitForSeconds(0.15f);
        attackEffect.SetActive(false);
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
            Invoke("EnableBlink", 0f);
            Invoke("DisableBlink", 0.2f);
            TakeDamage(1);
        }

        if (collision.collider.CompareTag("Spikes"))
        {
            currentHealth = 0;
            TakeDamage(0);
        }
    }

    public void FireBall()
    {
        fireBall.SetActive(true);
    }

    public void FireBallOff()
    {
        fireBall.SetActive(false);
    }    

    private void TakeDamage(int damage)
    {
        if (isPlayerInvulnerable) return;

        if (playerHealth > 0)
        {
            currentHealth -= damage;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            Invoke("EnableBlink", 0f);
            Invoke("DisableBlink", 0.2f);
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
    }

    private void EnableBlink()
    {
        blink.SetActive(true);
    }

    private void DisableBlink()
    {
        blink.SetActive(false);
    }
}
