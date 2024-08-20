using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int coinCount;
    public TextMeshProUGUI coinText;

    private void Update()
    {
        coinText.text = ": " + coinCount.ToString();
    }
}
