using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 2;
    int currentHealth;

    [SerializeField] private GameObject blink;

    private void Start()
    {
        currentHealth = maxHealth;
        blink.SetActive(false);
    }


    public void TakeDamage(int damage)
    {
        Invoke("EnableBlink", 0f);
        Invoke("DisableBlink", 0.3f);
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void Die()
    {
        Debug.Log("Balls");
    }

    private void EnableBlink()
    {
        blink.SetActive(true);
    }

    private void DisableBlink()
    {
        blink.SetActive(false);
    }
}
