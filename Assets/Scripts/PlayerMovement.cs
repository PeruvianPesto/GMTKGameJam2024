using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    [SerializeField] private float speed = 6f;
    [SerializeField] private float jumpPower = 14f;
    private bool isFacingRight = true;
    [SerializeField] private float circleRadius = 0.2f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private GameObject attackEffect;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackRate = 2f;
    [SerializeField] private AudioClip attackClip;
    private float nextAttackTime = 0f;
    private AudioSource audioSource;


    [SerializeField] private int playerHealth = 3;
    [SerializeField] private float invulnerabilityTime = 1f;
    private bool isPlayerInvulnerable = false;

    public GameObject[] hearts;

    [SerializeField] private GameObject blink;

    public Animator animator;
    public Animator hpAnimator;

    private void Start()
    {
        blink.SetActive(false);

        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();

        }

   
    }

    [Obsolete]
    private void Update()
    {

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
                nextAttackTime = Time.time + 1 / attackRate;
                hpAnimator.SetTrigger("TakeDamage");
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
        attackEffect.SetActive(true);
        AudioSource.PlayClipAtPoint(attackClip, transform.position, 0.5f);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }

        StartCoroutine(DisableAttackEffect());
    }

    private IEnumerator DisableAttackEffect()
    {
        yield return new WaitForSeconds(0.1f); 
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

        if (playerHealth > 0)
        {
            playerHealth -= damage;
            Destroy(hearts[playerHealth].gameObject);
        }

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
