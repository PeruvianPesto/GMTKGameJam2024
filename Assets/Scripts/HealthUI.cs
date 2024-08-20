using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image[] healthIcons;
    public Sprite fullHealthSprite;
    public Sprite emptyHealthSprite;

    private int currentHealth;

    public void UpdateHealthUI(int health)
    {
        currentHealth = health;

        for (int i = 0; i < healthIcons.Length; i++)
        {
            if (i < currentHealth)
            {
                healthIcons[i].sprite = fullHealthSprite;
            }
            else
            {
                healthIcons[i].sprite = emptyHealthSprite;
            }
        }
    }
}
