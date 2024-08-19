using System;
using UnityEngine;

public class Scalemite : MonoBehaviour
{
    public float moveSpeed = 2f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckDistance = 0.1f;
    public Transform edgeCheck;

    [SerializeField] private Rigidbody2D rb;
    private bool movingRight = true;

    private void Update()
    {
        Move();

        // Check for edges or downward slopes
        bool isGroundAhead = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        bool isEdgeAhead = !Physics2D.Raycast(edgeCheck.position, Vector2.down, groundCheckDistance, groundLayer);

        if (!isGroundAhead || isEdgeAhead)
        {
            TurnAround();
        }
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(movingRight ? moveSpeed : -moveSpeed, rb.linearVelocityY);
    }

    void TurnAround()
    {
        movingRight = !movingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}


