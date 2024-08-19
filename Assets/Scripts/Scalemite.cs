using System;
using UnityEngine;

public class Scalemite : MonoBehaviour
{
    public int speed = 3;
    public Rigidbody2D rb;
    private bool isFacingRight = true;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.linearVelocity = transform.right * speed;
    }

    private void Flip()
    {
        if (isFacingRight || !isFacingRight)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Flip();
            speed = speed * (-1);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Flip();
            speed = speed * (-1);
        }
    }
}


