using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject normalPlayer;
    public GameObject smallPlayer;
    public GameObject largePlayer;
    public CameraFollow cameraFollow; // Reference to the CameraFollow script

    private GameObject currentPlayer;
    private bool isSizeLocked = false;
    [SerializeField] private float sizeChangeTimer = 10f;
    [SerializeField] private float smallPlayerOffsetY = 0.5f; // Vertical offset for small player

    private void Start()
    {
        currentPlayer = normalPlayer;
        normalPlayer.SetActive(true);
        smallPlayer.SetActive(false);
        largePlayer.SetActive(false);
        if (cameraFollow != null)
        {
            cameraFollow.target = normalPlayer.transform; // Set initial target
        }
    }

    private void Update()
    {
        if (!isSizeLocked)
        {
            if (Input.GetMouseButtonDown(0)) // Left click for small
            {
                StartCoroutine(ChangeSize(smallPlayer, smallPlayerOffsetY));
            }
            else if (Input.GetMouseButtonDown(1)) // Right click for large
            {
                StartCoroutine(ChangeSize(largePlayer, 0f)); // No offset for large player
            }
        }
    }

    private IEnumerator ChangeSize(GameObject newPlayer, float verticalOffset)
    {
        isSizeLocked = true;

        // Switch to the new player object
        newPlayer.transform.position = currentPlayer.transform.position + new Vector3(0, verticalOffset, 0);
        currentPlayer.SetActive(false);
        newPlayer.SetActive(true);
        currentPlayer = newPlayer;

        // Update the camera's target
        if (cameraFollow != null)
        {
            cameraFollow.target = currentPlayer.transform;
        }

        // Wait for the duration of size change
        yield return new WaitForSeconds(sizeChangeTimer);

        // Revert back to normal size
        if (currentPlayer != normalPlayer)
        {
            currentPlayer.SetActive(false);
            normalPlayer.transform.position = currentPlayer.transform.position;
            normalPlayer.SetActive(true);
            currentPlayer = normalPlayer;

            // Update the camera's target
            if (cameraFollow != null)
            {
                cameraFollow.target = normalPlayer.transform;
            }
        }

        isSizeLocked = false;
    }
}
