using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SmallCharacterMovement : MonoBehaviour
{
    private float horizontal;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpPower = 20f;
    private bool isFacingRight = true;
    [SerializeField] private float circleRadius = 0.1f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private GameObject attackEffect;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private int attackDamage = 0;
    [SerializeField] private float attackRate = 2f;
    private float nextAttackTime = 0f;
    [SerializeField] private AudioClip coinCollectClip;
    private AudioSource audioSource;


    [SerializeField] private float playerHealth = 1f;
    [SerializeField] private float currentHealth;
    [SerializeField] private float invulnerabilityTime = 3f;
    private bool isPlayerInvulnerable = false;

    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image currentHealthBar;

    [SerializeField] private GameObject blink;

    public GameManager coinManager;

    private Vector3 lastCheckpointPosition;
    private void Start()
    {
        currentHealth = playerHealth;
        totalHealthBar.fillAmount = currentHealth / 3;

        blink.SetActive(false);

        audioSource = gameObject.GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();

        }
    }

    private void Update()
    {

        currentHealthBar.fillAmount = currentHealth / 3;

        horizontal = Input.GetAxisRaw("Horizontal");


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
        attackEffect.SetActive(true);

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Gold"))
        {
            Destroy(collision.gameObject);
            coinManager.coinCount++;
            audioSource.PlayOneShot(coinCollectClip);
        }
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

    private void EnableBlink()
    {
        blink.SetActive(true);
    }

    private void DisableBlink()
    {
        blink.SetActive(false);
    }

    private void Die()
    {
        Debug.Log("Player died");
        Respawn();
    }

    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        lastCheckpointPosition = checkpointPosition;
    }

    private void Respawn()
    {
        transform.position = lastCheckpointPosition;
        currentHealth = playerHealth; // Reset health if desired
        currentHealthBar.fillAmount = currentHealth / 3; // Update health UI
        StartCoroutine(PlayerInvulnerabilityCoroutine()); // Provide invulnerability after respawn
    }
}
