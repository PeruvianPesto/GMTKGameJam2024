using UnityEngine;

public class Tombstone : MonoBehaviour
{
    // Reference to the GameObject you want to activate
    public GameObject objectToActivate;

    // This method is called when another Collider2D enters the trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Set the GameObject to active
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }
    }
}
