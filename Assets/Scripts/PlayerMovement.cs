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

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D boxCollider;

    public Transform attackPoint;
    public float attackRange;
    public LayerMask enemyLayers;
    public int attackDamage = 1;

    private enum SizeState { Normal, Small, Large }
    private SizeState currentSizeState = SizeState.Normal;

    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite smallSprite;
    [SerializeField] private Sprite largeSprite;

    [SerializeField] private Vector2 normalColliderSize;
    [SerializeField] private Vector2 smallColliderSize;
    [SerializeField] private Vector2 largeColliderSize;

    private bool isSizeLocked = false;
    [SerializeField] private float sizeChangeTimer = 10f;
    private float timer = 0f;


    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

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

        if (!isSizeLocked)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                ChangeSize(SizeState.Small);
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                ChangeSize(SizeState.Large);
            }
        }
        if (isSizeLocked)
        {
            timer += Time.deltaTime;
            if (timer >= sizeChangeTimer)
            {
                RevertToNormalSize();
            }
        }

        Flip();
    }

    private void RevertToNormalSize()
    {
        currentSizeState = SizeState.Normal;
        spriteRenderer.sprite = normalSprite;
        boxCollider.size = normalColliderSize;
        speed = 6f; // Default speed
        jumpPower = 14f; // Default jump power

        isSizeLocked = false; // Unlock size change
    }

    private void ChangeSize(SizeState newSizeState)
    {
        if (newSizeState == currentSizeState) return;

        currentSizeState = newSizeState;

        switch (newSizeState)
        {
            case SizeState.Small:
                spriteRenderer.sprite = smallSprite;
                boxCollider.size = smallColliderSize;
                speed = 9f; // Adjust speed for smaller size
                jumpPower = 18f; // Adjust jump power for smaller size
                break;

            case SizeState.Large:
                spriteRenderer.sprite = largeSprite;
                boxCollider.size = largeColliderSize;
                speed = 4f; // Adjust speed for larger size
                jumpPower = 8f; // Adjust jump power for larger size
                break;
        }

        isSizeLocked = true;
        timer = 0f; // Reset the timer
    }

    private void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
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
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}
