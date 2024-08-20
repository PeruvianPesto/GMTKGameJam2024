using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement>().SetCheckpoint(transform.position);
        }
        if (other.CompareTag("BigPlayer"))
        {
            other.GetComponent<BigCharacterMovement>().SetCheckpoint(transform.position);
        }
        if (other.CompareTag("SmallPlayer"))
        {
            other.GetComponent<SmallCharacterMovement>().SetCheckpoint(transform.position); 
        }
    }
}