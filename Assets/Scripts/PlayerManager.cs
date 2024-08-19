using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject normalPlayer;
    public GameObject smallPlayer;
    public GameObject largePlayer;
    public CameraFollow cameraFollow; // Reference to the CameraFollow script

    private GameObject currentPlayer;

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

    public void ChangeSize(GameObject newPlayer, float verticalOffset)
    {
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
    }
}
