using UnityEngine;

public class Scalemite : MonoBehaviour
{
    public int miteHealth = 2;
    public int speed = 20;
    public Rigidbody2D rb;
    private bool isFacingRight = true;

    bool grounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = transform.right * speed;
    }

    private void Flip()
    {
        if (isFacingRight|| !isFacingRight)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }

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
            grounded = false;
        }
    }
}
