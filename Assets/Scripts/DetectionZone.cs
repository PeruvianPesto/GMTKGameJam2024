using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public Scalemite scalemite;

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            scalemite.Flip();
        }
    }
}
