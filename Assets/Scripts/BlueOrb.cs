using UnityEngine;

public class BlueOrb : MonoBehaviour
{
    [SerializeField] private float smallPlayerOffsetY = 0.5f;
    [SerializeField] private PlayerManager playerManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerManager.ChangeSize(playerManager.smallPlayer, smallPlayerOffsetY);
            playerManager.OrbCollected(gameObject); // Notify PlayerManager of the orb collection
            gameObject.SetActive(false); // Deactivate instead of destroying the orb
        }
    }
}