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

    public Transform attackPoint;
    public float attackRange;
    public LayerMask enemyLayers;
    public int attackDamage = 1; 

    public Animator animator; 

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        // --------Animation-related-------- 
        if (horizontal != 0f) { 
            animator.SetBool("AnimBool_isWalking", true); 
        } 
        else
        {
            animator.SetBool("AnimBool_isWalking", false);
        }
        if (IsGrounded() == true){
            animator.SetBool("AnimBool_isGrounded", true);
        }
        else{
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

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (IsGrounded() == true){
                animator.SetTrigger("AnimTrig_AttackGrounded"); 
            }
            else{
                animator.SetTrigger("AnimTrig_AttackAirborn"); 
            }
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
