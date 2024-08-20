using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{
    public GameObject button;
    public GameObject canvas;
    public GameObject player;
    public GameObject goodbye;
    public Animator animator;

    public float targetTime = 7.0f;

    void Update()
    {

        targetTime -= Time.deltaTime;

        if (targetTime <= 3.0f)
        {
            goodbye.SetActive(true);
        }

        if (targetTime <= 0.0f)
        {
            timerEnded();
        }
    }

    void timerEnded()
    {
        button.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        canvas.SetActive(true);
        player.SetActive(false);
    }

    public void Done()
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }
}
