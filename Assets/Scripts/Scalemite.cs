using System;
using UnityEngine;

public class Scalemite : MonoBehaviour
{
    public int speed = 3;
    public Rigidbody2D rb;
    private bool isFacingRight = true;
    private Transform groundPoint;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    public float circleRadius;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.linearVelocity = transform.right * speed;

        if(!IsGrounded())
        {
            Flip();
        }
    }

    public void Flip()
    {
        if (isFacingRight || !isFacingRight)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
            speed = speed * (-1);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Flip();
        }
    }

    private void OnDrawGizmos()
    {
        if (groundCheck == null) return;

        Gizmos.DrawWireSphere(groundCheck.position, circleRadius);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, circleRadius, groundLayer);
    }
}


