using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 2;
    int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }


    public void TakeDamage(int damage)
    {
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
}
