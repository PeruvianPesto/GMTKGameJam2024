using UnityEngine;

public class BlueOrb : MonoBehaviour
{
    [SerializeField] private float smallPlayerOffsetY = 0.5f; // Vertical offset for small player
    [SerializeField] private PlayerManager playerManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerManager.ChangeSize(playerManager.smallPlayer, smallPlayerOffsetY); ; // Change size to large player
            Destroy(gameObject); // Destroy the orb after picking it up
        }
    }
}