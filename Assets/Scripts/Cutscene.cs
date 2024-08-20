using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public GameObject canvas;
    public GameObject player;
    public Animator animator;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        canvas.SetActive(true);
        player.SetActive(false);
    }

    public void Done()
    {
        animator.SetTrigger("cutscene");
    }
}
