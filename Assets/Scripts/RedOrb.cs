using UnityEngine;

public class RedOrb : MonoBehaviour
{
    [SerializeField] private float bigPlayerOffsetY = 1f; // Vertical offset for big player
    [SerializeField] private PlayerManager playerManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerManager.ChangeSize(playerManager.largePlayer, bigPlayerOffsetY); // Change size to large player
            Destroy(gameObject); // Destroy the orb after picking it up
        }
    }
}