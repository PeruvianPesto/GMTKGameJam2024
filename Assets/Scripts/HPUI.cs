using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public TextMeshProUGUI healthText;

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + playerHealth.health;
    }
}
