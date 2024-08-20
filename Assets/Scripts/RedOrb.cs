using UnityEngine;

public class RedOrb : MonoBehaviour
{
    [SerializeField] private float bigPlayerOffsetY = 1f;
    [SerializeField] private PlayerManager playerManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerManager.ChangeSize(playerManager.largePlayer, bigPlayerOffsetY);
            playerManager.OrbCollected(gameObject); // Notify PlayerManager of the orb collection
            gameObject.SetActive(false); // Deactivate instead of destroying the orb
        }
    }
}