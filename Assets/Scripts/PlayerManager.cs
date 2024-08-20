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
    private Coroutine sizeChangeCoroutine;

    [SerializeField] private float powerUpCounter = 20f;

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
        if (Input.GetKeyDown(KeyCode.Escape))
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
        // Stop any ongoing size change coroutine
        if (sizeChangeCoroutine != null)
        {
            StopCoroutine(sizeChangeCoroutine);
        }

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

        // Start the coroutine to revert size after 20 seconds
        sizeChangeCoroutine = StartCoroutine(RevertSizeAfterDelay(powerUpCounter));
    }

    private IEnumerator RevertSizeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeSize(normalPlayer, 0f);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
