using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject normalPlayer;
    public GameObject smallPlayer;
    public GameObject largePlayer;
    public CameraFollow cameraFollow; // Reference to the CameraFollow script

    public GameObject canvas;
    public bool isPaused = false;

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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        canvas.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        canvas.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
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
